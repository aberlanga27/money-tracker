namespace MoneyTracker.API.Config;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using MoneyTracker.Domain.Entities.Config;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

/// <summary>
/// Logging configuration
/// </summary>
[ExcludeFromCodeCoverage]
public static class LoggingConfiguration
{
    /// <summary>
    /// Add the logging configuration
    /// </summary>
    /// <param name="host"></param>
    /// <param name="appSettings"></param>
    /// <returns></returns>
    public static IHostBuilder AddLoggingConfiguration(this IHostBuilder host, MoneyTrackerSettings appSettings)
    {
        var logEventLevel = Enum.Parse<LogEventLevel>(appSettings.Logging.LogLevel.Default);
        var logEventLevelMicrosoft = Enum.Parse<LogEventLevel>(appSettings.Logging.LogLevel.Microsoft);

        host.UseSerilog((context, logger) =>
        {
            logger
                .MinimumLevel.Is(logEventLevel)
                .MinimumLevel.Override("Microsoft", logEventLevelMicrosoft)
                .MinimumLevel.Override("System", logEventLevelMicrosoft)
                .MinimumLevel.Override("Microsoft.AspNetCore", logEventLevelMicrosoft)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", "MoneyTracker")
                .Enrich.WithProperty("Environment", appSettings.Environment?.Name ?? "Unknown")
                .Enrich.WithProperty("HostName", Dns.GetHostName())
                .WriteTo.Console(
                    formatProvider: CultureInfo.InvariantCulture
                )
                .WriteTo.Seq(
                    serverUrl: appSettings.Logging.Seq.BaseAddress,
                    apiKey: appSettings.Logging.Seq.ApiKey
                )
                .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore.Database.Command"))
            ;
        });

        return host;
    }
}