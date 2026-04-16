using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

// ================================== Opprett Build ==================================
var builder = WebApplication.CreateBuilder(args);

// ================================== Database konfigurasjon ==================================
var databaseConnection = builder.Configuration["DatabaseConnection"]
    ?? throw new InvalidOperationException("No DatabaseConnection in appsettings.json");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // For å opprette en MySQL connection må vi ha database-strengen og Pomelo må vite at det er MySql og hvilken utgave
    options.UseMySql(databaseConnection, 
        new MySqlServerVersion(new Version(9, 0, 0)));
});

// ================================== Swagger ==================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================================== Bygg Applikasjonen ==================================
var app = builder.Build();

// ================================== Minimal API endepunkter ==================================
// Henter alle produkter. Returnerer 200 med en liste med produkter eller tom liste
app.MapGet("/api/products", async (AppDbContext context) =>
    {
        var products = await context.Products.ToListAsync();
        return Results.Ok(products);
    })
    .WithName("GetAllProducts");

// Henter et produkt med ID. Returner Ok 200 med produktet eller Not Found 404
app.MapGet("/api/products/{id:int}", async (AppDbContext context, int id) => 
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        return product != null
            ? Results.Ok(product)
            : Results.NotFound();
    })
    .WithName("GetProduct");


// Helsesjekk
app.MapGet("/api/health", () =>
        Results.Ok("API OK"))
    .WithName("Healthcheck");

// ================================== Swagger ==================================
app.UseSwagger();
app.UseSwaggerUI();

// ================================== Kjør applikasjonen ==================================
app.Run();

// =======================================================================================
// =================================== Klasser og DTO ====================================
// =======================================================================================

/// <summary>
/// Product-modellen
/// </summary>
public class Product
{   
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name of product must be between 1-100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Brand is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Brand must be between 1-50 characters")]
    public string Brand { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public int Stock { get; set; }
}

/// <summary>
/// EFCore sin database-klasse. Vi registerer Products som en tabell
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    
}