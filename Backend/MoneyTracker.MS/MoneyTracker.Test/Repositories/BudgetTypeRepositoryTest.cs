namespace MoneyTracker.Test.Repositories;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Infrastructure.Repositories;
using MoneyTracker.Test.Utils;
using Moq;
using Xunit;

public class BudgetTypeRepositoryTest
{
    private readonly Mock<MoneyTrackerContext> mockContext;
    private readonly BudgetTypeRepository budgetTypeRepository;

    public BudgetTypeRepositoryTest()
    {
        mockContext = DbContextMockTools.GetMockedContext();
        budgetTypeRepository = new BudgetTypeRepository(mockContext.Object, new Mock<ILocalizationProvider>().Object);
    }

    [Fact]
    public async Task GetCount()
    {
        var result = await budgetTypeRepository.GetCount();

        Assert.IsType<int>(result);
    }

    [Fact]
    public void GetAllBudgetTypes()
    {
        var result = budgetTypeRepository.GetAllBudgetTypes();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BudgetType>>(result);
    }

    [Fact]
    public void GetAllBudgetTypes_WithPagination()
    {
        var result = budgetTypeRepository.GetAllBudgetTypes(0, 0);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BudgetType>>(result);
    }

    [Fact]
    public async Task GetBudgetTypeById()
    {
        var result = await budgetTypeRepository.GetBudgetTypeById(1);

        Assert.NotNull(result);
        Assert.IsType<BudgetType>(result);
    }

    [Fact]
    public async Task CreateBudgetType()
    {
        var budgetType = new BudgetType()
        {
            BudgetTypeId = 0,
            BudgetTypeName = "test_new",
            BudgetTypeDays = 2,
            // CTX: new-test-attribute, do not remove this line
        };

        var result = await budgetTypeRepository.CreateBudgetType(budgetType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.True(result.Status);
        Assert.Equal(budgetType, result.Response);
    }

    [Fact]
    public async Task CreateBudgetType_InvalidUK()
    {
        var budgetType = new BudgetType()
        {
            BudgetTypeId = 0,
            BudgetTypeName = "test",
            BudgetTypeDays = 1,
            // CTX: test-attribute, do not remove this line
        };

        var result = await budgetTypeRepository.CreateBudgetType(budgetType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateBudgetType_InvalidFK()
    {
        var budgetType = new BudgetType()
        {
            BudgetTypeId = 0,
            BudgetTypeName = null!,
            BudgetTypeDays = -1,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await budgetTypeRepository.CreateBudgetType(budgetType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateBudgetType_Update()
    {
        var budgetType = new BudgetType()
        {
            BudgetTypeId = 1,
            BudgetTypeName = "test",
            BudgetTypeDays = 1,
            // CTX: test-attribute, do not remove this line
        };

        var result = await budgetTypeRepository.CreateBudgetType(budgetType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.True(result.Status);
        Assert.Equal(budgetType, result.Response);
    }

    [Fact]
    public async Task UpdateBudgetType()
    {
        var budgetType = new BudgetType()
        {
            BudgetTypeId = 1,
            BudgetTypeName = "test",
            BudgetTypeDays = 1,
            // CTX: test-attribute, do not remove this line
        };

        var result = await budgetTypeRepository.UpdateBudgetType(budgetType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.True(result.Status);
        Assert.Equal(budgetType, result.Response);
    }

    [Fact]
    public async Task UpdateBudgetType_InvalidUK()
    {
        var budgetType = new BudgetType()
        {
            BudgetTypeId = 2,
            BudgetTypeName = "test",
            BudgetTypeDays = 1,
            // CTX: test-attribute, do not remove this line
        };

        var result = await budgetTypeRepository.UpdateBudgetType(budgetType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateBudgetType_InvalidFK()
    {
        var budgetType = new BudgetType()
        {
            BudgetTypeId = 2,
            BudgetTypeName = null!,
            BudgetTypeDays = -1,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await budgetTypeRepository.UpdateBudgetType(budgetType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateBudgetType_NotFound()
    {
        var budgetType = new BudgetType()
        {
            BudgetTypeId = 3,
            BudgetTypeName = "test",
            BudgetTypeDays = 1,
            // CTX: test-attribute, do not remove this line
        };

        var result = await budgetTypeRepository.UpdateBudgetType(budgetType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteBudgetType()
    {
        var result = await budgetTypeRepository.DeleteBudgetType(2);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteBudgetType_InvalidFK()
    {
        var result = await budgetTypeRepository.DeleteBudgetType(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteBudgetType_NotFound()
    {
        var result = await budgetTypeRepository.DeleteBudgetType(3);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchBudgetTypes()
    {
        var result = budgetTypeRepository.SearchBudgetTypes("1");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BudgetType>>(result);
    }

    [Fact]
    public void GetBudgetTypesByAttributes()
    {
        var result = budgetTypeRepository.GetBudgetTypesByAttributes(new BudgetType
        {
            BudgetTypeId = 1,
            BudgetTypeName = "test",
            BudgetTypeDays = 1,
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BudgetType>>(result);
    }
}