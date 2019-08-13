using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TodoAPI.Checks
{
    public class SelfHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new HealthCheckResult(
                HealthStatus.Healthy,
                description: "API up!"));
        }
    }
}