namespace MoneyTracker.API.Middlewares;

using Ardalis.GuardClauses;
using MoneyTracker.API.Extensions;
using MoneyTracker.Domain.Entities.Config;

/// <summary>
/// Middleware to process headers
/// </summary>
public class HeadersMiddleware(
    RequestDelegate next,
    MoneyTrackerSettings appSettings,
    ApiConfiguration apiConfiguration
)
{
    private readonly RequestDelegate next = Guard.Against.Null(next);
    private readonly MoneyTrackerSettings appSettings = Guard.Against.Null(appSettings);
    private readonly ApiConfiguration apiConfiguration = Guard.Against.Null(apiConfiguration);

    /// <summary>
    /// Invoke the middleware to process headers
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        var request = context.Request;
        apiConfiguration.Language = request.GetHeaderValue("Api-Language", out var languageHeader)
            ? languageHeader
            : appSettings.Localization.Default;

        await next(context);
    }
}