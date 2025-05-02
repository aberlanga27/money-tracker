namespace MoneyTracker.Test.Filters;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using MoneyTracker.API.Filters;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;
using Moq;
using Xunit;

public class HttpGlobalExceptionFilterTest
{
    private readonly MoneyTrackerSettings mockSettings;
    private readonly Mock<ILogger<HttpGlobalExceptionFilter>> mockLogger;
    private readonly HttpGlobalExceptionFilter exceptionFilter;

    public HttpGlobalExceptionFilterTest()
    {
        mockLogger = new Mock<ILogger<HttpGlobalExceptionFilter>>();

        mockSettings = new MoneyTrackerSettings
        {
            Environment = new MoneyTrackerSettings.EnvironmentModel
            {
                Name = "Development"
            }
        };

        exceptionFilter = new HttpGlobalExceptionFilter(mockSettings, mockLogger.Object);
    }

    [Fact]
    public void OnException_ShouldHandleExceptionAndSetResult()
    {
        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };
        var context = new ExceptionContext(actionContext, [])
        {
            Exception = new ArgumentException("Test exception")
        };

        exceptionFilter.OnException(context);

        Assert.True(context.ExceptionHandled);
        var result = Assert.IsType<ObjectResult>(context.Result);
        var valueResponse = Assert.IsType<ValueResponse<string>>(result.Value);
        Assert.False(valueResponse.Status);
        Assert.Equal("Test exception", valueResponse.Message);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    }

    [Fact]
    public void OnException_ShouldHideStackTraceInProduction()
    {
        mockSettings.Environment.Name = "Production";

        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };
        var context = new ExceptionContext(actionContext, [])
        {
            Exception = new ArgumentException("Test exception")
        };

        exceptionFilter.OnException(context);

        Assert.True(context.ExceptionHandled);
        var result = Assert.IsType<ObjectResult>(context.Result);
        var valueResponse = Assert.IsType<ValueResponse<string>>(result.Value);
        Assert.False(valueResponse.Status);
        Assert.Equal("Test exception", valueResponse.Message);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    }
}