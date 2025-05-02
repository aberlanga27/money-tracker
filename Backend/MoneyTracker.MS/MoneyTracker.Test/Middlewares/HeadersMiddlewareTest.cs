namespace MoneyTracker.Test.Middlewares;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoneyTracker.API.Middlewares;
using MoneyTracker.Domain.Entities.Config;
using Moq;
using Xunit;
using static MoneyTracker.Domain.Entities.Config.MoneyTrackerSettings;

public class HeadersMiddlewareTest
{
    private readonly Mock<RequestDelegate> nextMock;
    private readonly Mock<HttpContext> contextMock;
    private readonly Mock<HttpRequest> requestMock;
    private readonly MoneyTrackerSettings appSettings;
    private readonly ApiConfiguration apiConfiguration;

    public HeadersMiddlewareTest()
    {
        nextMock = new Mock<RequestDelegate>();
        contextMock = new Mock<HttpContext>();
        requestMock = new Mock<HttpRequest>();
        contextMock.Setup(c => c.Request).Returns(requestMock.Object);
        appSettings = new MoneyTrackerSettings
        {
            Localization = new LocalizationModel { Default = "en-US" }
        };
        apiConfiguration = new ApiConfiguration();
    }

    [Fact]
    public async Task Invoke_WithApiLanguageHeader_SetsApiConfigurationLanguage()
    {
        var middleware = new HeadersMiddleware(nextMock.Object, appSettings, apiConfiguration);
        requestMock.Setup(r => r.Headers["Api-Language"]).Returns("es-MX");

        await middleware.Invoke(contextMock.Object);

        Assert.Equal("es-MX", apiConfiguration.Language);
        nextMock.Verify(next => next(contextMock.Object), Times.Once);
    }

    [Fact]
    public async Task Invoke_WithoutApiLanguageHeader_SetsApiConfigurationLanguageToDefault()
    {
        var middleware = new HeadersMiddleware(nextMock.Object, appSettings, apiConfiguration);
        requestMock.Setup(r => r.Headers["Api-Language"]).Returns((string)null!);

        await middleware.Invoke(contextMock.Object);

        Assert.Equal("en-US", apiConfiguration.Language);
        nextMock.Verify(next => next(contextMock.Object), Times.Once);
    }
}