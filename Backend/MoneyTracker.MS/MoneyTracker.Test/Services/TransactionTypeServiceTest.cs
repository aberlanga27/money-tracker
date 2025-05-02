namespace MoneyTracker.Test.Services;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Services;
using Moq;
using Xunit;
using static MoneyTracker.Domain.Entities.Config.MoneyTrackerSettings;

public class TransactionTypeServiceTest
{
    private readonly Mock<ICacheProvider> cacheProvider = new();
    private readonly Mock<ITransactionTypeRepository> transactionTypeRepository = new();
    private readonly TransactionTypeService transactionTypeService;
    private readonly ValueResponse<TransactionType> okDefaultResponse = new()
    {
        Status = true,
        Message = "Ok",
        Response = new TransactionType { }
    };
    private readonly ValueResponse<TransactionType> errorDefaultResponse = new()
    {
        Status = false,
        Message = "Error"
    };

    public TransactionTypeServiceTest()
    {
        var appSettings = new Mock<MoneyTrackerSettings>();
        appSettings.Object.Pagination = new PaginationModel { DefaultPageSize = 10, MaxPageSize = 100 };

        transactionTypeService = new TransactionTypeService(
            appSettings.Object,
            new Mock<ILocalizationProvider>().Object,
            cacheProvider.Object,
            transactionTypeRepository.Object
        );
    }

    [Fact]
    public void GetAllTransactionTypes()
    {
        transactionTypeRepository.Setup(x => x.GetAllTransactionTypes(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = transactionTypeService.GetAllTransactionTypes();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionTypeDTO>>(result);
    }

    [Fact]
    public async Task GetAllTransactionTypes_WithPagination()
    {
        transactionTypeRepository.Setup(x => x.GetAllTransactionTypes(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = await transactionTypeService.GetAllTransactionTypes(null, null);

        Assert.NotNull(result);
        Assert.IsType<PaginationResponse<IEnumerable<TransactionTypeDTO>>>(result);
    }

    [Fact]
    public async Task GetTransactionTypeById()
    {
        transactionTypeRepository.Setup(x => x.GetTransactionTypeById(It.IsAny<int>()))
            .ReturnsAsync(new TransactionType());

        var result = await transactionTypeService.GetTransactionTypeById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionTypeDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetTransactionTypeById_Cache()
    {
        cacheProvider.Setup(x => x.GetAsync<TransactionTypeDTO>(It.IsAny<string>()))
            .ReturnsAsync(new TransactionTypeDTO());

        var result = await transactionTypeService.GetTransactionTypeById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionTypeDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetTransactionTypeById_InvalidId()
    {
        transactionTypeRepository.Setup(x => x.GetTransactionTypeById(It.IsAny<int>()))
            .ReturnsAsync((TransactionType?)null);

        var result = await transactionTypeService.GetTransactionTypeById(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionTypeDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateTransactionType()
    {
        transactionTypeRepository.Setup(x => x.CreateTransactionType(It.IsAny<TransactionType>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await transactionTypeService.CreateTransactionType(new TransactionTypeDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionTypeDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task CreateTransactionType_InvalidData()
    {
        transactionTypeRepository.Setup(x => x.CreateTransactionType(It.IsAny<TransactionType>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await transactionTypeService.CreateTransactionType(new TransactionTypeDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionTypeDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateTransactionType()
    {
        transactionTypeRepository.Setup(x => x.UpdateTransactionType(It.IsAny<TransactionType>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await transactionTypeService.UpdateTransactionType(new TransactionTypeDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionTypeDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task UpdateTransactionType_InvalidData()
    {
        transactionTypeRepository.Setup(x => x.UpdateTransactionType(It.IsAny<TransactionType>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await transactionTypeService.UpdateTransactionType(new TransactionTypeDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionTypeDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteTransactionType()
    {
        transactionTypeRepository.Setup(x => x.DeleteTransactionType(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<TransactionType> { Status = true, Response = new TransactionType { } });

        var result = await transactionTypeService.DeleteTransactionType(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionTypeDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteTransactionType_InvalidId()
    {
        transactionTypeRepository.Setup(x => x.DeleteTransactionType(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<TransactionType> { Status = false });

        var result = await transactionTypeService.DeleteTransactionType(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionTypeDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchTransactionTypes()
    {
        transactionTypeRepository.Setup(x => x.SearchTransactionTypes(It.IsAny<string>()))
            .Returns([]);

        var result = transactionTypeService.SearchTransactionTypes("search");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionTypeDTO>>(result);
    }

    [Fact]
    public void GetTransactionTypesByAttributes()
    {
        transactionTypeRepository.Setup(x => x.GetTransactionTypesByAttributes(It.IsAny<TransactionType>()))
            .Returns([]);

        var result = transactionTypeService.GetTransactionTypesByAttributes(new TransactionTypeDTO { });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionTypeDTO>>(result);
    }
}