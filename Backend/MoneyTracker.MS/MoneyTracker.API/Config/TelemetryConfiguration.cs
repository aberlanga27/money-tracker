// namespace MoneyTracker.API.Config;

// using System.Diagnostics.CodeAnalysis;
// using OpenTelemetry.Metrics;

// <PackageReference Include="OpenTelemetry.Exporter.Prometheus.AspNetCore" Version="1.9.0-beta.1" />
// <PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.9.0" />
// app.MapPrometheusScrapingEndpoint();

// [ExcludeFromCodeCoverage]
// public static class TelemetryConfiguration
// {
//     public static IServiceCollection AddPrometheusOpenTelemetry(this IServiceCollection services)
//     {
//         services.AddOpenTelemetry()
//             .WithMetrics(x =>
//             {
//                 x.AddPrometheusExporter();
//                 x.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
//                 x.AddView(
//                     "request-duration",
//                     new ExplicitBucketHistogramConfiguration
//                     {
//                         Boundaries = [0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10],
//                     }
//                 );
//             });

//         return services;
//     }
// }