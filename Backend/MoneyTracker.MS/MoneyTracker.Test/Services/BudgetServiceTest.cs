namespace MoneyTracker.Test.Services;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Services;
using Moq;
using Xunit;
using static MoneyTracker.Domain.Entities.Config.MoneyTrackerSettings;

public class BudgetServiceTest
{
    private readonly Mock<ICacheProvider> cacheProvider = new();
    private readonly Mock<IBudgetRepository> budgetRepository = new();
    private readonly BudgetService budgetService;
    private readonly ValueResponse<Budget> okDefaultResponse = new()
    {
        Status = true,
        Message = "Ok",
        Response = new Budget { }
    };
    private readonly ValueResponse<Budget> errorDefaultResponse = new()
    {
        Status = false,
        Message = "Error"
    };

    public BudgetServiceTest()
    {
        var appSettings = new Mock<MoneyTrackerSettings>();
        appSettings.Object.Pagination = new PaginationModel { DefaultPageSize = 10, MaxPageSize = 100 };

        budgetService = new BudgetService(
            appSettings.Object,
            new Mock<ILocalizationProvider>().Object,
            cacheProvider.Object,
            budgetRepository.Object
        );
    }

    [Fact]
    public void GetAllBudgets()
    {
        budgetRepository.Setup(x => x.GetAllBudgets(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = budgetService.GetAllBudgets();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BudgetDTO>>(result);
    }

    [Fact]
    public async Task GetAllBudgets_WithPagination()
    {
        budgetRepository.Setup(x => x.GetAllBudgets(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = await budgetService.GetAllBudgets(null, null);

        Assert.NotNull(result);
        Assert.IsType<PaginationResponse<IEnumerable<BudgetDTO>>>(result);
    }

    [Fact]
    public async Task GetBudgetById()
    {
        budgetRepository.Setup(x => x.GetBudgetById(It.IsAny<int>()))
            .ReturnsAsync(new Budget());

        var result = await budgetService.GetBudgetById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetBudgetById_Cache()
    {
        cacheProvider.Setup(x => x.GetAsync<BudgetDTO>(It.IsAny<string>()))
            .ReturnsAsync(new BudgetDTO());

        var result = await budgetService.GetBudgetById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetBudgetById_InvalidId()
    {
        budgetRepository.Setup(x => x.GetBudgetById(It.IsAny<int>()))
            .ReturnsAsync((Budget?)null);

        var result = await budgetService.GetBudgetById(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateBudget()
    {
        budgetRepository.Setup(x => x.CreateBudget(It.IsAny<Budget>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await budgetService.CreateBudget(new BudgetDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task CreateBudget_InvalidData()
    {
        budgetRepository.Setup(x => x.CreateBudget(It.IsAny<Budget>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await budgetService.CreateBudget(new BudgetDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateBudget()
    {
        budgetRepository.Setup(x => x.UpdateBudget(It.IsAny<Budget>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await budgetService.UpdateBudget(new BudgetDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task UpdateBudget_InvalidData()
    {
        budgetRepository.Setup(x => x.UpdateBudget(It.IsAny<Budget>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await budgetService.UpdateBudget(new BudgetDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteBudget()
    {
        budgetRepository.Setup(x => x.DeleteBudget(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<Budget> { Status = true, Response = new Budget { } });

        var result = await budgetService.DeleteBudget(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteBudget_InvalidId()
    {
        budgetRepository.Setup(x => x.DeleteBudget(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<Budget> { Status = false });

        var result = await budgetService.DeleteBudget(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchBudgets()
    {
        budgetRepository.Setup(x => x.SearchBudgets(It.IsAny<string>()))
            .Returns([]);

        var result = budgetService.SearchBudgets("search");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BudgetDTO>>(result);
    }

    [Fact]
    public void GetBudgetsByAttributes()
    {
        budgetRepository.Setup(x => x.GetBudgetsByAttributes(It.IsAny<Budget>()))
            .Returns([]);

        var result = budgetService.GetBudgetsByAttributes(new BudgetDTO { });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BudgetDTO>>(result);
    }
}