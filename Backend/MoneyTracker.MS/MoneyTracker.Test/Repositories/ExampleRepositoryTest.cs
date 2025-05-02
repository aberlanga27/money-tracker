namespace MoneyTracker.Test.Repositories;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Infrastructure.Repositories;
using MoneyTracker.Test.Utils;
using Moq;
using Xunit;

public class ExampleRepositoryTest
{
    private readonly Mock<MoneyTrackerContext> mockContext;
    private readonly ExampleRepository exampleRepository;

    public ExampleRepositoryTest()
    {
        mockContext = DbContextMockTools.GetMockedContext();
        exampleRepository = new ExampleRepository(mockContext.Object, new Mock<ILocalizationProvider>().Object);
    }

    [Fact]
    public async Task GetCount()
    {
        var result = await exampleRepository.GetCount();

        Assert.IsType<int>(result);
    }

    [Fact]
    public void GetAllExamples()
    {
        var result = exampleRepository.GetAllExamples();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Example>>(result);
    }

    [Fact]
    public void GetAllExamples_WithPagination()
    {
        var result = exampleRepository.GetAllExamples(0, 0);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Example>>(result);
    }

    [Fact]
    public async Task GetExampleById()
    {
        var result = await exampleRepository.GetExampleById(1);

        Assert.NotNull(result);
        Assert.IsType<Example>(result);
    }

    [Fact]
    public async Task CreateExample()
    {
        var example = new Example()
        {
            ExampleId = 0,
            // CTX: new-test-attribute, do not remove this line
        };

        var result = await exampleRepository.CreateExample(example);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.True(result.Status);
        Assert.Equal(example, result.Response);
    }

    [Fact]
    public async Task CreateExample_InvalidUK()
    {
        var example = new Example()
        {
            ExampleId = 0,
            // CTX: test-attribute, do not remove this line
        };

        var result = await exampleRepository.CreateExample(example);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateExample_InvalidFK()
    {
        var example = new Example()
        {
            ExampleId = 0,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await exampleRepository.CreateExample(example);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateExample_Update()
    {
        var example = new Example()
        {
            ExampleId = 1,
            // CTX: test-attribute, do not remove this line
        };

        var result = await exampleRepository.CreateExample(example);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.True(result.Status);
        Assert.Equal(example, result.Response);
    }

    [Fact]
    public async Task UpdateExample()
    {
        var example = new Example()
        {
            ExampleId = 1,
            // CTX: test-attribute, do not remove this line
        };

        var result = await exampleRepository.UpdateExample(example);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.True(result.Status);
        Assert.Equal(example, result.Response);
    }

    [Fact]
    public async Task UpdateExample_InvalidUK()
    {
        var example = new Example()
        {
            ExampleId = 2,
            // CTX: test-attribute, do not remove this line
        };

        var result = await exampleRepository.UpdateExample(example);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateExample_InvalidFK()
    {
        var example = new Example()
        {
            ExampleId = 2,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await exampleRepository.UpdateExample(example);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateExample_NotFound()
    {
        var example = new Example()
        {
            ExampleId = 3,
            // CTX: test-attribute, do not remove this line
        };

        var result = await exampleRepository.UpdateExample(example);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteExample()
    {
        var result = await exampleRepository.DeleteExample(2);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteExample_InvalidFK()
    {
        var result = await exampleRepository.DeleteExample(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteExample_NotFound()
    {
        var result = await exampleRepository.DeleteExample(3);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Example>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchExamples()
    {
        var result = exampleRepository.SearchExamples("1");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Example>>(result);
    }

    [Fact]
    public void GetExamplesByAttributes()
    {
        var result = exampleRepository.GetExamplesByAttributes(new Example
        {
            ExampleId = 1,
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Example>>(result);
    }
}