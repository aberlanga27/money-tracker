namespace MoneyTracker.API.Filters;

using Ardalis.GuardClauses;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MoneyTracker.Domain.Entities.Config;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Global OpenAPI filter to add global headers
/// </summary>
public class HttpHeadersOperationFilter(
    MoneyTrackerSettings appSettings
) : IOperationFilter
{
    private readonly MoneyTrackerSettings appSettings = Guard.Against.Null(appSettings);

    /// <summary>
    /// Add the headers to the operation
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Api-Language",
            In = ParameterLocation.Header,
            Required = false,
            Description = "The language to be used for the response content",
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new OpenApiString(appSettings.Localization.Default)
            }
        });
    }
}