namespace MoneyTracker.API.Config;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.API.Workers;

/// <summary>
/// Workers configuration
/// </summary>
[ExcludeFromCodeCoverage]
public static class WorkersConfiguration
{
    /// <summary>
    /// Add the hosted services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.Configure<HostOptions>(options =>
        {
            options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });

        services
            .AddHostedService<SampleWorker>()
        ;

        return services;
    }
}