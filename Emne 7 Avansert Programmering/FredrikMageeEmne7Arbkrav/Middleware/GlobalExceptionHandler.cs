using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace FredrikMageeEmne7Arbkrav.Middleware;

/// <summary>
/// Middleware som fanger opp alle uhåndterte exceptions slik at programmet ikke stopper når en exception kastes.
/// Dette gir oss mer kontroll over hva som skjer når exceptions kastest, ved å logge feilen og returnere
/// et detaljert ProblemDetails-objekt. Det betyr at alle responsene får samme oppsett
/// </summary>
/// <param name="logger"></param>
internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Denne metoden returnerer en bool etter vi har håndtert en feil, true hvis vi har håndtert eller false hvis ikke.
    /// Hvis vi returnerer false, så vil vi returnere en HTTP 500 til brukeren. Må implementeres når vi 
    /// arver fra IExceptionHandler
    /// </summary>
    /// <param name="httpContext">HTTP-requesten slik at vi kan hente ut data fra forespørselen og lage responsen</param>
    /// <param name="exception">Exceptionen som har oppstått som vi nå håndterere</param>
    /// <param name="cancellationToken">Denne brukes for å avbryte requesten, kan brukes i asynkrone operasjoner</param>
    /// <returns>True hvis har håndtert det, false hvis ikke</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Vi logger alle exceptions
        logger.LogError(exception, "GlobalExceptionHandler: Exception occurred: {Message}", exception.Message);
        
        // Bruker en switch på exceptionen, og lager en tuple utifra hvilken feil som har oppstått 
        var (status, title, details) = exception switch
        {
            ArgumentException ex => (StatusCodes.Status400BadRequest, "Bad Request", ex.Message),
            KeyNotFoundException ex=> (StatusCodes.Status404NotFound, "Not found",
                $"The requested resource was not found. {ex.Message}" ),
            ValidationException ex => (StatusCodes.Status400BadRequest, "Bad Request",
                ex.Message),
            // Her kan vi fange opp SQLite Error 19 'UNIQUE constraint failed: Books.ISB' som skjer når vi prøver å
            // lagre en ISBN som allerede eksisterer
            DbUpdateException { InnerException: SqliteException { SqliteErrorCode: 19 } } => 
                (StatusCodes.Status409Conflict, "Conflict", "Book with this ISBN already exists"),
            // Fanger opp resten av database-errorene
            DbUpdateException ex => (StatusCodes.Status500InternalServerError, "Database error", ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "Server Error", "An unexpected error occurred.")
        };
        
        // Vi mapper til et ProblemDetails-objekt med tuplen vår. Dette objektet er en standarisert måte å
        // returnere exceptions på
        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = details,
        };
        
        // Her legger vi til statuskoden vår i responsen
        httpContext.Response.StatusCode = status;
        
        // Her serialiserer vi ProblemDetails-objektet slik at det blir en oversiktelig feilmelding returnert som JSON
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        
        // Feilen er håndtert
        return true;
    }
}