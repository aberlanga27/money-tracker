namespace MoneyTracker.API.Config;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.API.Common;
using MoneyTracker.Domain.Utils;

/// <summary>
/// SignalR configuration
/// </summary>
[ExcludeFromCodeCoverage]
public static class SignalRConfiguration
{
    /// <summary>
    /// Map the SignalR hubs
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication MapSignalRHubs(this WebApplication app)
    {
        app.MapHub<SignalRHub>($"{Constants.BasePath}/endpoint/path")
            .RequireCors("DefaultPolicy");

        return app;
    }
}