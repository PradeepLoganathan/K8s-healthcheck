using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

public class LivenessProbe : IHealthCheck
{
    private readonly ILogger<LivenessProbe> _logger;

    public LivenessProbe(ILogger<LivenessProbe> logger)
    {
        _logger = logger;
    }
    
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
       bool isLive = HealthStatusController.IsLive; // Replace with actual readiness logic
        if (isLive)
        {
            _logger.LogInformation("Received a request at the /liveness probe.");
            return Task.FromResult(HealthCheckResult.Healthy("Application is live"));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("Application is not live"));
    }    
    
}