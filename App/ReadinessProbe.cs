using Microsoft.Extensions.Diagnostics.HealthChecks;

public class ReadinessProbe : IHealthCheck
{
    private readonly ILogger<LivenessProbe> _logger;

    public ReadinessProbe(ILogger<LivenessProbe> logger)
    {
        _logger = logger;
    }
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        bool isReady = HealthStatusController.IsReady; // Replace with actual readiness logic
        if (isReady)
        {
            _logger.LogInformation("Received a request at the /readiness probe.");
            return Task.FromResult(HealthCheckResult.Healthy("Application is ready"));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("Application is not ready"));
    }
}