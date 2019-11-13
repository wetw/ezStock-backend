using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace ezStock.HealthChecks
{
    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly IOptionsMonitor<MemoryCheckOptions> _options;

        public MemoryHealthCheck(IOptionsMonitor<MemoryCheckOptions> options)
        {
            _options = options;
        }

        public string Name => "memory_check";

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var options = _options.Get(context.Registration.Name);

            // Include GC information in the reported diagnostics.
            var allocated = GC.GetTotalMemory(false);
            var data = new Dictionary<string, object>() {
                { "AllocatedBytes", allocated },
                { "Gen0Collections", GC.CollectionCount (0) },
                { "Gen1Collections", GC.CollectionCount (1) },
                { "Gen2Collections", GC.CollectionCount (2) },
            };

            var status = allocated < options.Threshold ?
                HealthStatus.Healthy : HealthStatus.Unhealthy;

            return Task.FromResult(new HealthCheckResult(
                status,
                $"Reports degraded status if allocated >= {options.Threshold.Bytes().Humanize(CultureInfo.CurrentCulture)}.",
                null,
                data));
        }
    }

    public class MemoryCheckOptions
    {
        // Failure threshold (in bytes)
        public long Threshold { get; set; } = 1024L * 1024L * 1024L;
    }
}
