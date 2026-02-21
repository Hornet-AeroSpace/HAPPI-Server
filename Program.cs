using HAPPI.Shared;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SensorService>(); // Register SensorService as a singleton to share state across requests
builder.Services.AddHostedService(provider => provider.GetRequiredService<SensorService>()); // Start the SensorService as a background service
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Telemetry Endpoint: Returns the latest snapshot from the Sensor Service
app.MapGet("/telemetry", (SensorService sensors) => 
{
    return sensors.CurrentTelemetry; 
})
.WithName("GetTelemetry");

// Video Placeholder, will update later with actual video streaming logic
app.MapGet("/video-stream", () => "Video stream not yet implemented.");

app.Run("http://0.0.0.0:5097"); //Default port to 5097