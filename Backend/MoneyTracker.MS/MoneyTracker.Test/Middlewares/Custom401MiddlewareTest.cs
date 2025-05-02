namespace MoneyTracker.Test.Middlewares;

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoneyTracker.API.Middlewares;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Interfaces;
using Moq;
using Newtonsoft.Json;
using Xunit;

public class Custom401MiddlewareTest
{
    private readonly Mock<RequestDelegate> nextMock;
    private readonly Mock<ILocalizationProvider> translatorMock;
    private readonly Custom401Middleware middleware;

    public Custom401MiddlewareTest()
    {
        nextMock = new Mock<RequestDelegate>();
        translatorMock = new Mock<ILocalizationProvider>();
        translatorMock.Setup(t => t.T(It.IsAny<string>())).Returns((string s) => s);
        middleware = new Custom401Middleware(nextMock.Object, translatorMock.Object);
    }

    [Fact]
    public async Task Invoke_StatusCode401_ManageUnauthorizedAccessCalled()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        nextMock.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Callback<HttpContext>(ctx => ctx.Response.StatusCode = 401);

        await middleware.Invoke(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
        var managedResponse = JsonConvert.DeserializeObject<ValueResponse<string>>(responseBody);

        Assert.Equal(401, context.Response.StatusCode);
        Assert.Equal("Unauthorized", managedResponse!.Message);
        Assert.Equal("Please provide a valid token", managedResponse.Response);
    }

    [Fact]
    public async Task Invoke_StatusCodeNot401_ManageUnauthorizedAccessNotCalled()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        nextMock.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Callback<HttpContext>(ctx => ctx.Response.StatusCode = 200);

        await middleware.Invoke(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();

        Assert.Equal(200, context.Response.StatusCode);
        Assert.Empty(responseBody);
    }
}