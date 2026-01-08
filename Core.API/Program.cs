using Asp.Versioning;
using Microsoft.Extensions.Options;
using Notes.Configuration;
using Notes.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddNotesModule(new("234"));
builder.Services.AddControllers();

// Api versioning Setup
builder.Services.AddApiVersioning(options =>
{
    // If no version provided Assume default API is called 
    options.AssumeDefaultVersionWhenUnspecified=true;
    // Default api version
    options.DefaultApiVersion= new(1,0);

    // On responce report the versions of api present for clients to be aware about versions 
    options.ReportApiVersions=true;
    
    // Strategy to read verisons 
    options.ApiVersionReader = new UrlSegmentApiVersionReader();

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
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
