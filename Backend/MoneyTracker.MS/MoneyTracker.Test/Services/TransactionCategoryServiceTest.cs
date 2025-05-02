namespace MoneyTracker.Test.Services;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Services;
using Moq;
using Xunit;
using static MoneyTracker.Domain.Entities.Config.MoneyTrackerSettings;

public class TransactionCategoryServiceTest
{
    private readonly Mock<ICacheProvider> cacheProvider = new();
    private readonly Mock<ITransactionCategoryRepository> transactionCategoryRepository = new();
    private readonly TransactionCategoryService transactionCategoryService;
    private readonly ValueResponse<TransactionCategory> okDefaultResponse = new()
    {
        Status = true,
        Message = "Ok",
        Response = new TransactionCategory { }
    };
    private readonly ValueResponse<TransactionCategory> errorDefaultResponse = new()
    {
        Status = false,
        Message = "Error"
    };

    public TransactionCategoryServiceTest()
    {
        var appSettings = new Mock<MoneyTrackerSettings>();
        appSettings.Object.Pagination = new PaginationModel { DefaultPageSize = 10, MaxPageSize = 100 };

        transactionCategoryService = new TransactionCategoryService(
            appSettings.Object,
            new Mock<ILocalizationProvider>().Object,
            cacheProvider.Object,
            transactionCategoryRepository.Object
        );
    }

    [Fact]
    public void GetAllTransactionCategorys()
    {
        transactionCategoryRepository.Setup(x => x.GetAllTransactionCategorys(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = transactionCategoryService.GetAllTransactionCategorys();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionCategoryDTO>>(result);
    }

    [Fact]
    public async Task GetAllTransactionCategorys_WithPagination()
    {
        transactionCategoryRepository.Setup(x => x.GetAllTransactionCategorys(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = await transactionCategoryService.GetAllTransactionCategorys(null, null);

        Assert.NotNull(result);
        Assert.IsType<PaginationResponse<IEnumerable<TransactionCategoryDTO>>>(result);
    }

    [Fact]
    public async Task GetTransactionCategoryById()
    {
        transactionCategoryRepository.Setup(x => x.GetTransactionCategoryById(It.IsAny<int>()))
            .ReturnsAsync(new TransactionCategory());

        var result = await transactionCategoryService.GetTransactionCategoryById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategoryDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetTransactionCategoryById_Cache()
    {
        cacheProvider.Setup(x => x.GetAsync<TransactionCategoryDTO>(It.IsAny<string>()))
            .ReturnsAsync(new TransactionCategoryDTO());

        var result = await transactionCategoryService.GetTransactionCategoryById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategoryDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetTransactionCategoryById_InvalidId()
    {
        transactionCategoryRepository.Setup(x => x.GetTransactionCategoryById(It.IsAny<int>()))
            .ReturnsAsync((TransactionCategory?)null);

        var result = await transactionCategoryService.GetTransactionCategoryById(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategoryDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateTransactionCategory()
    {
        transactionCategoryRepository.Setup(x => x.CreateTransactionCategory(It.IsAny<TransactionCategory>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await transactionCategoryService.CreateTransactionCategory(new TransactionCategoryDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategoryDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task CreateTransactionCategory_InvalidData()
    {
        transactionCategoryRepository.Setup(x => x.CreateTransactionCategory(It.IsAny<TransactionCategory>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await transactionCategoryService.CreateTransactionCategory(new TransactionCategoryDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategoryDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateTransactionCategory()
    {
        transactionCategoryRepository.Setup(x => x.UpdateTransactionCategory(It.IsAny<TransactionCategory>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await transactionCategoryService.UpdateTransactionCategory(new TransactionCategoryDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategoryDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task UpdateTransactionCategory_InvalidData()
    {
        transactionCategoryRepository.Setup(x => x.UpdateTransactionCategory(It.IsAny<TransactionCategory>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await transactionCategoryService.UpdateTransactionCategory(new TransactionCategoryDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategoryDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteTransactionCategory()
    {
        transactionCategoryRepository.Setup(x => x.DeleteTransactionCategory(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<TransactionCategory> { Status = true, Response = new TransactionCategory { } });

        var result = await transactionCategoryService.DeleteTransactionCategory(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategoryDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteTransactionCategory_InvalidId()
    {
        transactionCategoryRepository.Setup(x => x.DeleteTransactionCategory(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<TransactionCategory> { Status = false });

        var result = await transactionCategoryService.DeleteTransactionCategory(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategoryDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchTransactionCategorys()
    {
        transactionCategoryRepository.Setup(x => x.SearchTransactionCategorys(It.IsAny<string>()))
            .Returns([]);

        var result = transactionCategoryService.SearchTransactionCategorys("search");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionCategoryDTO>>(result);
    }

    [Fact]
    public void GetTransactionCategorysByAttributes()
    {
        transactionCategoryRepository.Setup(x => x.GetTransactionCategorysByAttributes(It.IsAny<TransactionCategory>()))
            .Returns([]);

        var result = transactionCategoryService.GetTransactionCategorysByAttributes(new TransactionCategoryDTO { });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionCategoryDTO>>(result);
    }
}