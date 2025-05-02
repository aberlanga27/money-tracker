namespace MoneyTracker.Test.Services;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Services;
using Moq;
using Xunit;
using static MoneyTracker.Domain.Entities.Config.MoneyTrackerSettings;

public class BudgetTypeServiceTest
{
    private readonly Mock<ICacheProvider> cacheProvider = new();
    private readonly Mock<IBudgetTypeRepository> budgetTypeRepository = new();
    private readonly BudgetTypeService budgetTypeService;
    private readonly ValueResponse<BudgetType> okDefaultResponse = new()
    {
        Status = true,
        Message = "Ok",
        Response = new BudgetType { }
    };
    private readonly ValueResponse<BudgetType> errorDefaultResponse = new()
    {
        Status = false,
        Message = "Error"
    };

    public BudgetTypeServiceTest()
    {
        var appSettings = new Mock<MoneyTrackerSettings>();
        appSettings.Object.Pagination = new PaginationModel { DefaultPageSize = 10, MaxPageSize = 100 };

        budgetTypeService = new BudgetTypeService(
            appSettings.Object,
            new Mock<ILocalizationProvider>().Object,
            cacheProvider.Object,
            budgetTypeRepository.Object
        );
    }

    [Fact]
    public void GetAllBudgetTypes()
    {
        budgetTypeRepository.Setup(x => x.GetAllBudgetTypes(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = budgetTypeService.GetAllBudgetTypes();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BudgetTypeDTO>>(result);
    }

    [Fact]
    public async Task GetAllBudgetTypes_WithPagination()
    {
        budgetTypeRepository.Setup(x => x.GetAllBudgetTypes(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = await budgetTypeService.GetAllBudgetTypes(null, null);

        Assert.NotNull(result);
        Assert.IsType<PaginationResponse<IEnumerable<BudgetTypeDTO>>>(result);
    }

    [Fact]
    public async Task GetBudgetTypeById()
    {
        budgetTypeRepository.Setup(x => x.GetBudgetTypeById(It.IsAny<int>()))
            .ReturnsAsync(new BudgetType());

        var result = await budgetTypeService.GetBudgetTypeById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetTypeDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetBudgetTypeById_Cache()
    {
        cacheProvider.Setup(x => x.GetAsync<BudgetTypeDTO>(It.IsAny<string>()))
            .ReturnsAsync(new BudgetTypeDTO());

        var result = await budgetTypeService.GetBudgetTypeById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetTypeDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetBudgetTypeById_InvalidId()
    {
        budgetTypeRepository.Setup(x => x.GetBudgetTypeById(It.IsAny<int>()))
            .ReturnsAsync((BudgetType?)null);

        var result = await budgetTypeService.GetBudgetTypeById(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetTypeDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateBudgetType()
    {
        budgetTypeRepository.Setup(x => x.CreateBudgetType(It.IsAny<BudgetType>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await budgetTypeService.CreateBudgetType(new BudgetTypeDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetTypeDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task CreateBudgetType_InvalidData()
    {
        budgetTypeRepository.Setup(x => x.CreateBudgetType(It.IsAny<BudgetType>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await budgetTypeService.CreateBudgetType(new BudgetTypeDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetTypeDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateBudgetType()
    {
        budgetTypeRepository.Setup(x => x.UpdateBudgetType(It.IsAny<BudgetType>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await budgetTypeService.UpdateBudgetType(new BudgetTypeDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetTypeDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task UpdateBudgetType_InvalidData()
    {
        budgetTypeRepository.Setup(x => x.UpdateBudgetType(It.IsAny<BudgetType>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await budgetTypeService.UpdateBudgetType(new BudgetTypeDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetTypeDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteBudgetType()
    {
        budgetTypeRepository.Setup(x => x.DeleteBudgetType(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<BudgetType> { Status = true, Response = new BudgetType { } });

        var result = await budgetTypeService.DeleteBudgetType(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetTypeDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteBudgetType_InvalidId()
    {
        budgetTypeRepository.Setup(x => x.DeleteBudgetType(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<BudgetType> { Status = false });

        var result = await budgetTypeService.DeleteBudgetType(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BudgetTypeDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchBudgetTypes()
    {
        budgetTypeRepository.Setup(x => x.SearchBudgetTypes(It.IsAny<string>()))
            .Returns([]);

        var result = budgetTypeService.SearchBudgetTypes("search");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BudgetTypeDTO>>(result);
    }

    [Fact]
    public void GetBudgetTypesByAttributes()
    {
        budgetTypeRepository.Setup(x => x.GetBudgetTypesByAttributes(It.IsAny<BudgetType>()))
            .Returns([]);

        var result = budgetTypeService.GetBudgetTypesByAttributes(new BudgetTypeDTO { });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BudgetTypeDTO>>(result);
    }
}