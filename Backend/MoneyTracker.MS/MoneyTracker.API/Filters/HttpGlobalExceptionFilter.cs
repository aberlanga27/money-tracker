namespace MoneyTracker.API.Filters;

using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;

/// <summary>
/// Global exception filter on MVC
/// </summary>
public class HttpGlobalExceptionFilter(
    MoneyTrackerSettings appSettings,
    ILogger<HttpGlobalExceptionFilter> logger
) : IExceptionFilter
{
    private readonly MoneyTrackerSettings appSettings = Guard.Against.Null(appSettings);
    private readonly ILogger<HttpGlobalExceptionFilter> logger = Guard.Against.Null(logger);

    /// <summary>
    /// On exception handle error
    /// </summary>
    /// <param name="context"></param>
    public void OnException(ExceptionContext context)
    {
        logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);
        context.ExceptionHandled = true;

        var managedResponse = new ValueResponse<string>()
        {
            Status = false,
            Message = context.Exception.Message,
            Response = appSettings.Environment?.Name != "Production"
                ? context.Exception.StackTrace
                : null
        };

        context.Result = new ObjectResult(managedResponse)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}