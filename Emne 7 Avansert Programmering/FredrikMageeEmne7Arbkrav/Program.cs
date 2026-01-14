using FredrikMageeEmne7Arbkrav.Data;
using FredrikMageeEmne7Arbkrav.Repositories;
using FredrikMageeEmne7Arbkrav.Services;
using FredrikMageeEmne7Arbkrav.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ========================================== Konfigurasjon ==========================================
// Bruker Serilogger
builder.Host.UseSerilog();

// Innstillinger til loggeren vår
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()                          // Skriver til konsoll
    .WriteTo.File(                              // Skriver til fil
        path: "logs/app-.log",                  // Ønskede sti
        rollingInterval: RollingInterval.Day,   // Vi lager en ny fil til hver dag
        retainedFileCountLimit: 7)              // Maks 7 filer om gangen
    .CreateLogger();

// ========================================== DI-registering ==========================================
// Exception-håndtering
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Database
builder.Services.AddDbContext<BookDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DatabaseConnection"));
});

// Kontrollere
builder.Services.AddControllers();

// Forretningslogikk
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ========================================== App/Middleware ==========================================
var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Feil- og logghåndtering
app.UseExceptionHandler();
app.UseSerilogRequestLogging();

// Kontrollere
app.MapControllers();

// Starter applikasjonen
app.Run();