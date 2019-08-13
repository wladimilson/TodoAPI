using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TodoAPI.Checks
{
    public static class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddSelfCheck(this IHealthChecksBuilder builder, string name, HealthStatus? failureStatus = null, IEnumerable<string> tags = null)
        {
            // Register a check of type SelfHealthCheck
            builder.AddCheck<SelfHealthCheck>(name, failureStatus ?? HealthStatus.Degraded, tags);

            return builder;
        }
    }
}