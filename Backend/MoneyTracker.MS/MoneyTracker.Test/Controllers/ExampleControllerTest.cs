namespace MoneyTracker.Test.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyTracker.API.Controllers;
using MoneyTracker.API.Validator;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Interfaces;
using Moq;
using Xunit;

public class ExampleControllerTest
{
    private readonly ExampleController exampleController;

    public ExampleControllerTest()
    {
        exampleController = new ExampleController(
            new Mock<ILogger<ExampleController>>().Object,
            new Mock<ILocalizationProvider>().Object,
            new Mock<IExampleService>().Object,
            new ExampleValidator()
        );
    }

    [Fact]
    public async Task GetAll()
    {
        var result = await exampleController.GetAll(null, null);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById()
    {
        var result = await exampleController.GetById(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_InvalidId()
    {
        var result = await exampleController.GetById(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create()
    {
        var result = await exampleController.Create(new ExampleDTO
        {
            ExampleId = 1,
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_InvalidData()
    {
        var result = await exampleController.Create(new ExampleDTO
        {
            ExampleId = -1,
            // CTX: invalid-test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update()
    {
        var result = await exampleController.Update(new ExampleDTO
        {
            ExampleId = 1,
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_InvalidData()
    {
        var result = await exampleController.Update(new ExampleDTO { ExampleId = -1 });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete()
    {
        var result = await exampleController.Delete(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_InvalidId()
    {
        var result = await exampleController.Delete(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Search()
    {
        var result = exampleController.Search("example");

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Search_InvalidQuery()
    {
        var result = exampleController.Search(null!);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void GetByAttributes()
    {
        var result = exampleController.GetByAttributes(new ExampleDTO
        {
            ExampleId = 1
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
}