namespace MoneyTracker.API.Middlewares;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Interfaces;
using Newtonsoft.Json;

/// <summary>
/// Unauthorized middleware
/// </summary>
public class Custom401Middleware(
    RequestDelegate next,
    ILocalizationProvider translator
)
{
    private readonly RequestDelegate next = Guard.Against.Null(next);
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);

    private async Task ManageUnauthorizedAccess(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 401;

        var managedResponse = new ValueResponse<string>()
        {
            Status = false,
            Message = translator.T("Unauthorized"),
            Response = translator.T("Please provide a valid token")
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(managedResponse));
    }

    /// <summary>
    /// Invoke the middleware to manage the unauthorized access
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == 401)
            await ManageUnauthorizedAccess(context);
    }
}