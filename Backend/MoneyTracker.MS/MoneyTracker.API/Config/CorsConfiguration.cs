namespace MoneyTracker.API.Config;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.Domain.Entities.Config;

/// <summary>
/// CORS configuration
/// </summary>
[ExcludeFromCodeCoverage]
public static class CorsConfiguration
{
    /// <summary>
    /// Add the CORS configuration
    /// </summary>
    /// <param name="services"></param>
    /// <param name="appSettings"></param>
    /// <returns></returns>
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, MoneyTrackerSettings appSettings)
    {
        var allowedOrigins = appSettings.AllowedOrigins ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy", builder =>
            {
                builder
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}