namespace MoneyTracker.API.Config;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.API.Middlewares;

/// <summary>
/// Middleware configuration
/// </summary>
[ExcludeFromCodeCoverage]
public static class MiddlewareConfiguration
{
    /// <summary>
    /// Add the middlewares
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddSignalR();

        return services;
    }

    /// <summary>
    /// Use the custom middlewares
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAppMiddlewares(this IApplicationBuilder builder)
    {
        return builder
            .UseMiddleware<HeadersMiddleware>()
            .UseMiddleware<Custom401Middleware>()
            .UseAuthentication()
            .UseAuthorization()
        ;
    }
}