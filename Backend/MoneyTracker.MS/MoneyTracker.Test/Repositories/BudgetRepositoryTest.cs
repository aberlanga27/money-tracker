namespace MoneyTracker.Test.Repositories;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Infrastructure.Repositories;
using MoneyTracker.Test.Utils;
using Moq;
using Xunit;

public class BudgetRepositoryTest
{
    private readonly Mock<MoneyTrackerContext> mockContext;
    private readonly BudgetRepository budgetRepository;

    public BudgetRepositoryTest()
    {
        mockContext = DbContextMockTools.GetMockedContext();
        budgetRepository = new BudgetRepository(mockContext.Object, new Mock<ILocalizationProvider>().Object);
    }

    [Fact]
    public async Task GetCount()
    {
        var result = await budgetRepository.GetCount();

        Assert.IsType<int>(result);
    }

    [Fact]
    public void GetAllBudgets()
    {
        var result = budgetRepository.GetAllBudgets();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Budget>>(result);
    }

    [Fact]
    public void GetAllBudgets_WithPagination()
    {
        var result = budgetRepository.GetAllBudgets(0, 0);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Budget>>(result);
    }

    [Fact]
    public async Task GetBudgetById()
    {
        var result = await budgetRepository.GetBudgetById(1);

        Assert.NotNull(result);
        Assert.IsType<Budget>(result);
    }

    [Fact]
    public async Task CreateBudget()
    {
        var budget = new Budget()
        {
            BudgetId = 0,
            TransactionCategoryId = 2,
            BudgetTypeId = 2,
            BudgetAmount = 2.0m,
            // CTX: new-test-attribute, do not remove this line
        };

        var result = await budgetRepository.CreateBudget(budget);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.True(result.Status);
        Assert.Equal(budget, result.Response);
    }

    [Fact]
    public async Task CreateBudget_InvalidUK()
    {
        var budget = new Budget()
        {
            BudgetId = 0,
            TransactionCategoryId = 1,
            BudgetTypeId = 1,
            BudgetAmount = 1.0m,
            // CTX: test-attribute, do not remove this line
        };

        var result = await budgetRepository.CreateBudget(budget);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateBudget_InvalidFK()
    {
        var budget = new Budget()
        {
            BudgetId = 0,
            TransactionCategoryId = -1,
            BudgetTypeId = -1,
            BudgetAmount = -1.0m,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await budgetRepository.CreateBudget(budget);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateBudget_Update()
    {
        var budget = new Budget()
        {
            BudgetId = 1,
            TransactionCategoryId = 1,
            BudgetTypeId = 1,
            BudgetAmount = 1.0m,
            // CTX: test-attribute, do not remove this line
        };

        var result = await budgetRepository.CreateBudget(budget);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.True(result.Status);
        Assert.Equal(budget, result.Response);
    }

    [Fact]
    public async Task UpdateBudget()
    {
        var budget = new Budget()
        {
            BudgetId = 1,
            TransactionCategoryId = 1,
            BudgetTypeId = 1,
            BudgetAmount = 1.0m,
            // CTX: test-attribute, do not remove this line
        };

        var result = await budgetRepository.UpdateBudget(budget);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.True(result.Status);
        Assert.Equal(budget, result.Response);
    }

    [Fact]
    public async Task UpdateBudget_InvalidUK()
    {
        var budget = new Budget()
        {
            BudgetId = 2,
            TransactionCategoryId = 1,
            BudgetTypeId = 1,
            BudgetAmount = 1.0m,
            // CTX: test-attribute, do not remove this line
        };

        var result = await budgetRepository.UpdateBudget(budget);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateBudget_InvalidFK()
    {
        var budget = new Budget()
        {
            BudgetId = 2,
            TransactionCategoryId = -1,
            BudgetTypeId = -1,
            BudgetAmount = -1.0m,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await budgetRepository.UpdateBudget(budget);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateBudget_NotFound()
    {
        var budget = new Budget()
        {
            BudgetId = 3,
            TransactionCategoryId = 1,
            BudgetTypeId = 1,
            BudgetAmount = 1.0m,
            // CTX: test-attribute, do not remove this line
        };

        var result = await budgetRepository.UpdateBudget(budget);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteBudget()
    {
        var result = await budgetRepository.DeleteBudget(2);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteBudget_InvalidFK()
    {
        var result = await budgetRepository.DeleteBudget(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteBudget_NotFound()
    {
        var result = await budgetRepository.DeleteBudget(3);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Budget>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchBudgets()
    {
        var result = budgetRepository.SearchBudgets("1");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Budget>>(result);
    }

    [Fact]
    public void GetBudgetsByAttributes()
    {
        var result = budgetRepository.GetBudgetsByAttributes(new Budget
        {
            BudgetId = 1,
            TransactionCategoryId = 1,
            BudgetTypeId = 1,
            BudgetAmount = 1.0m,
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Budget>>(result);
    }
}