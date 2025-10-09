var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var states = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

int altitude = ((int)DateTime.UnixEpoch.Ticks % 6); // Reported in meters, conversion takes place on the client side
float attitudeX = 0; // Reported in degrees
float attitudeY = 0; // Reported in degrees
float attitudeZ = 0; // Reported in degrees
float acceleration = 0; // Reported in G's, converted on the client side

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            states[Random.Shared.Next(states.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/altitude", () =>
{
    int[] currentAltitude = new int[1];
    currentAltitude[0] = (int)DateTime.UnixEpoch.Ticks % 6;
    return currentAltitude;
})
.WithName("GetAltitude");

app.Run("http://0.0.0.0:5097");

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
