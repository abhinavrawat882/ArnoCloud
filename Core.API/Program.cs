using Asp.Versioning;
using Microsoft.Extensions.Options;
using Notes.Configuration;
using Notes.Service;

var builder = WebApplication.CreateBuilder(args);

// --- 1. ADD CORS SERVICES ---
// This tells .NET to allow requests from other domains (like your local machine)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Allows request from any source (localhost, etc.)
              .AllowAnyMethod()  // Allows GET, POST, PUT, DELETE, etc.
              .AllowAnyHeader(); // Allows custom headers
    });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNotesModule(new("234"));
builder.Services.AddControllers();

builder.Services.Configure<JwtOption>(
    builder.Configuration.GetSection("Jwt")
);

// Api versioning Setup
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// --- 2. SWAGGER CONFIGURATION ---
// I moved this OUTSIDE the 'if (IsDevelopment)' block.
// Now Swagger will load even when deployed to Azure (which defaults to Production).
app.UseSwagger();
app.UseSwaggerUI();

// --- 3. ENABLE CORS MIDDLEWARE ---
// This must be placed AFTER UseHttpsRedirection and BEFORE MapControllers
app.UseCors("AllowAll");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}