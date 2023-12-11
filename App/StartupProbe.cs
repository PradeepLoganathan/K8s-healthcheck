using Microsoft.Extensions.Diagnostics.HealthChecks;

public class StartupProbe : IHealthCheck
{
    private readonly ILogger<LivenessProbe> _logger;

    public StartupProbe(ILogger<LivenessProbe> logger)
    {
        _logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Received a request at the /startup probe.");
        return Task.FromResult(HealthCheckResult.Healthy("Application has started up"));
    }
}