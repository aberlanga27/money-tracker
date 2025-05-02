namespace MoneyTracker.Test.Services;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Services;
using Moq;
using Xunit;
using static MoneyTracker.Domain.Entities.Config.MoneyTrackerSettings;

public class ExampleServiceTest
{
    private readonly Mock<ICacheProvider> cacheProvider = new();
    private readonly Mock<IExampleRepository> exampleRepository = new();
    private readonly ExampleService exampleService;
    private readonly ValueResponse<Example> okDefaultResponse = new()
    {
        Status = true,
        Message = "Ok",
        Response = new Example { }
    };
    private readonly ValueResponse<Example> errorDefaultResponse = new()
    {
        Status = false,
        Message = "Error"
    };

    public ExampleServiceTest()
    {
        var appSettings = new Mock<MoneyTrackerSettings>();
        appSettings.Object.Pagination = new PaginationModel { DefaultPageSize = 10, MaxPageSize = 100 };

        exampleService = new ExampleService(
            appSettings.Object,
            new Mock<ILocalizationProvider>().Object,
            cacheProvider.Object,
            exampleRepository.Object
        );
    }

    [Fact]
    public void GetAllExamples()
    {
        exampleRepository.Setup(x => x.GetAllExamples(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = exampleService.GetAllExamples();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ExampleDTO>>(result);
    }

    [Fact]
    public async Task GetAllExamples_WithPagination()
    {
        exampleRepository.Setup(x => x.GetAllExamples(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = await exampleService.GetAllExamples(null, null);

        Assert.NotNull(result);
        Assert.IsType<PaginationResponse<IEnumerable<ExampleDTO>>>(result);
    }

    [Fact]
    public async Task GetExampleById()
    {
        exampleRepository.Setup(x => x.GetExampleById(It.IsAny<int>()))
            .ReturnsAsync(new Example());

        var result = await exampleService.GetExampleById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<ExampleDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetExampleById_Cache()
    {
        cacheProvider.Setup(x => x.GetAsync<ExampleDTO>(It.IsAny<string>()))
            .ReturnsAsync(new ExampleDTO());

        var result = await exampleService.GetExampleById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<ExampleDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetExampleById_InvalidId()
    {
        exampleRepository.Setup(x => x.GetExampleById(It.IsAny<int>()))
            .ReturnsAsync((Example?)null);

        var result = await exampleService.GetExampleById(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<ExampleDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateExample()
    {
        exampleRepository.Setup(x => x.CreateExample(It.IsAny<Example>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await exampleService.CreateExample(new ExampleDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<ExampleDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task CreateExample_InvalidData()
    {
        exampleRepository.Setup(x => x.CreateExample(It.IsAny<Example>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await exampleService.CreateExample(new ExampleDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<ExampleDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateExample()
    {
        exampleRepository.Setup(x => x.UpdateExample(It.IsAny<Example>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await exampleService.UpdateExample(new ExampleDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<ExampleDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task UpdateExample_InvalidData()
    {
        exampleRepository.Setup(x => x.UpdateExample(It.IsAny<Example>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await exampleService.UpdateExample(new ExampleDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<ExampleDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteExample()
    {
        exampleRepository.Setup(x => x.DeleteExample(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<Example> { Status = true, Response = new Example { } });

        var result = await exampleService.DeleteExample(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<ExampleDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteExample_InvalidId()
    {
        exampleRepository.Setup(x => x.DeleteExample(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<Example> { Status = false });

        var result = await exampleService.DeleteExample(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<ExampleDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchExamples()
    {
        exampleRepository.Setup(x => x.SearchExamples(It.IsAny<string>()))
            .Returns([]);

        var result = exampleService.SearchExamples("search");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ExampleDTO>>(result);
    }

    [Fact]
    public void GetExamplesByAttributes()
    {
        exampleRepository.Setup(x => x.GetExamplesByAttributes(It.IsAny<Example>()))
            .Returns([]);

        var result = exampleService.GetExamplesByAttributes(new ExampleDTO { });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ExampleDTO>>(result);
    }
}