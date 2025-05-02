namespace MoneyTracker.Test.Filters;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MoneyTracker.API.Filters;
using MoneyTracker.Domain.Entities.Config;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

public class HttpHeadersOperationFilterTest
{
    private readonly MoneyTrackerSettings mockSettings;
    private readonly HttpHeadersOperationFilter operationFilter;

    public HttpHeadersOperationFilterTest()
    {
        mockSettings = new MoneyTrackerSettings
        {
            Localization = new MoneyTrackerSettings.LocalizationModel
            {
                Default = "en-US"
            }
        };

        operationFilter = new HttpHeadersOperationFilter(mockSettings);
    }

    [Fact]
    public void Apply_ShouldAddApiLanguageHeader()
    {
        var operation = new OpenApiOperation();
        var context = new OperationFilterContext(null, null, null, null);

        operationFilter.Apply(operation, context);

        Assert.NotNull(operation.Parameters);
        var parameter = Assert.Single(operation.Parameters);
        Assert.Equal("Api-Language", parameter.Name);
        Assert.Equal(ParameterLocation.Header, parameter.In);
        Assert.False(parameter.Required);
        Assert.Equal("The language to be used for the response content", parameter.Description);
        Assert.Equal("string", parameter.Schema.Type);
        Assert.Equal("en-US", ((OpenApiString)parameter.Schema.Default).Value);
    }

    [Fact]
    public void Apply_ShouldNotOverrideExistingParameters()
    {
        var operation = new OpenApiOperation
        {
            Parameters =
            [
                new OpenApiParameter
                {
                    Name = "Existing-Header",
                    In = ParameterLocation.Header,
                    Required = true
                }
            ]
        };
        var context = new OperationFilterContext(null, null, null, null);

        operationFilter.Apply(operation, context);

        Assert.NotNull(operation.Parameters);
        Assert.Equal(2, operation.Parameters.Count);
        Assert.Contains(operation.Parameters, p => p.Name == "Existing-Header");
        Assert.Contains(operation.Parameters, p => p.Name == "Api-Language");
    }
}