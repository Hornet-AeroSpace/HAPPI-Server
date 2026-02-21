using HAPPI.Shared;

public class SensorService : BackgroundService
{
    // Thread-safe storage for the latest packet
    public TelemetryData CurrentTelemetry { get; private set; } = default!;
    
    private readonly string[] _states = 
    {
        // Disconnected: No data received, no connection to radio hardware
        // Pad Idle: Connected and receiving data, but rocket is still on the pad / default state when telemetry is not yet available
        // Ignition: Rocket has just launched, high acceleration, low altitude
        // Ascent: Rocket is ascending, high altitude, high velocity
        // Apogee: Rocket has reached its highest point, velocity near zero
        // Descent: Rocket is descending, decreasing altitude, increasing velocity
        // Landed: Rocket has landed, low altitude, low velocity
        // Error, Check Systems: Telemetry data is erratic or indicates a problem (e.g. negative altitude, battery voltage too low, etc.)
        "Disconnected", "Pad Idle", "Ignition", "Ascent", "Apogee", "Descent", "Landed", "Error, Check Systems"
    };

    // Stopping token is triggered when the application is shutting down
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var startTime = DateTime.UtcNow;

        while (!stoppingToken.IsCancellationRequested)
        {
            // SIMULATION LOGIC: Replace this block later with radio communication logic to read telemetry data from the rocket
            
            double timeElapsed = (DateTime.UtcNow - startTime).TotalSeconds;
            
            // Simulate a rocket flight (Sine wave for altitude)
            double altitude = Math.Max(0, 500 * Math.Sin(timeElapsed * 0.1)); 
            
            CurrentTelemetry = new TelemetryData(
                Timestamp: DateTime.Now.Ticks,
                MissionState: _states[Random.Shared.Next(0, _states.Length)],
                Altitude: altitude,
                VelocityZ: 50 * Math.Cos(timeElapsed * 0.1),
                AccelG: (float)(1.0 + (Random.Shared.NextDouble() * 2)),
                Pitch: (float)(Random.Shared.NextDouble() * 5),
                Roll: (float)(timeElapsed % 360),
                Yaw: 0,
                BatteryVoltageRocket: 12.4 - (timeElapsed * 0.001),
                BatteryVoltageGround: 12.4 - (timeElapsed * 0.001)
            );

            // Update rate: 10Hz (100ms)
            await Task.Delay(100, stoppingToken);
        }
    }
}