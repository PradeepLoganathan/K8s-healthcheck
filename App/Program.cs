using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);


// Add services to the container.
builder.Services.AddHealthChecks()
    .AddCheck<StartupProbe>(nameof(StartupProbe), tags: new[] {"startup"})
    .AddCheck<LivenessProbe>(nameof(LivenessProbe), tags: new[] {"liveness"})
    .AddCheck<ReadinessProbe>(nameof(ReadinessProbe), tags: new[] {"readiness"});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("Received a request at the root endpoint.");
    return "Hello World!";
});

app.MapGet("/hello", (ILogger<Program> logger) =>
{
    logger.LogInformation("Received a request at the /hello endpoint.");
    return "Hello from Minimal API!";
});

app.MapPost("/healthz/status", (bool? isLive, bool? isReady) =>
{
    string responseMessage = "";

    if (isLive.HasValue)
    {
        HealthStatusController.IsLive = isLive.Value;
        responseMessage += $"Liveness set to: {isLive.Value}. ";
    }

    if (isReady.HasValue)
    {
        HealthStatusController.IsReady = isReady.Value;
        responseMessage += $"Readiness set to: {isReady.Value}. ";
    }

    return responseMessage != "" ? Results.Ok(responseMessage) : Results.BadRequest("No valid parameters provided.");
});

app.MapGet("/healthz/status", () => new 
{
    IsLive = HealthStatusController.IsLive,
    IsReady = HealthStatusController.IsReady
});


app.MapHealthChecks("/healthz/startup", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("startup")
});

app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("readiness")
});

app.MapHealthChecks("/healthz/live", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("liveness")
});


app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.Run();
