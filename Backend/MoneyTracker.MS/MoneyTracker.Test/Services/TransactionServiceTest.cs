namespace MoneyTracker.Test.Services;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Services;
using Moq;
using Xunit;
using static MoneyTracker.Domain.Entities.Config.MoneyTrackerSettings;

public class TransactionServiceTest
{
    private readonly Mock<ICacheProvider> cacheProvider = new();
    private readonly Mock<ITransactionRepository> transactionRepository = new();
    private readonly TransactionService transactionService;
    private readonly ValueResponse<Transaction> okDefaultResponse = new()
    {
        Status = true,
        Message = "Ok",
        Response = new Transaction { }
    };
    private readonly ValueResponse<Transaction> errorDefaultResponse = new()
    {
        Status = false,
        Message = "Error"
    };

    public TransactionServiceTest()
    {
        var appSettings = new Mock<MoneyTrackerSettings>();
        appSettings.Object.Pagination = new PaginationModel { DefaultPageSize = 10, MaxPageSize = 100 };

        transactionService = new TransactionService(
            appSettings.Object,
            new Mock<ILocalizationProvider>().Object,
            cacheProvider.Object,
            transactionRepository.Object
        );
    }

    [Fact]
    public void GetAllTransactions()
    {
        transactionRepository.Setup(x => x.GetAllTransactions(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = transactionService.GetAllTransactions();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionDTO>>(result);
    }

    [Fact]
    public async Task GetAllTransactions_WithPagination()
    {
        transactionRepository.Setup(x => x.GetAllTransactions(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = await transactionService.GetAllTransactions(null, null);

        Assert.NotNull(result);
        Assert.IsType<PaginationResponse<IEnumerable<TransactionDTO>>>(result);
    }

    [Fact]
    public async Task GetTransactionById()
    {
        transactionRepository.Setup(x => x.GetTransactionById(It.IsAny<int>()))
            .ReturnsAsync(new Transaction());

        var result = await transactionService.GetTransactionById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetTransactionById_Cache()
    {
        cacheProvider.Setup(x => x.GetAsync<TransactionDTO>(It.IsAny<string>()))
            .ReturnsAsync(new TransactionDTO());

        var result = await transactionService.GetTransactionById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetTransactionById_InvalidId()
    {
        transactionRepository.Setup(x => x.GetTransactionById(It.IsAny<int>()))
            .ReturnsAsync((Transaction?)null);

        var result = await transactionService.GetTransactionById(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateTransaction()
    {
        transactionRepository.Setup(x => x.CreateTransaction(It.IsAny<Transaction>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await transactionService.CreateTransaction(new TransactionDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task CreateTransaction_InvalidData()
    {
        transactionRepository.Setup(x => x.CreateTransaction(It.IsAny<Transaction>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await transactionService.CreateTransaction(new TransactionDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateTransaction()
    {
        transactionRepository.Setup(x => x.UpdateTransaction(It.IsAny<Transaction>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await transactionService.UpdateTransaction(new TransactionDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task UpdateTransaction_InvalidData()
    {
        transactionRepository.Setup(x => x.UpdateTransaction(It.IsAny<Transaction>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await transactionService.UpdateTransaction(new TransactionDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteTransaction()
    {
        transactionRepository.Setup(x => x.DeleteTransaction(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<Transaction> { Status = true, Response = new Transaction { } });

        var result = await transactionService.DeleteTransaction(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteTransaction_InvalidId()
    {
        transactionRepository.Setup(x => x.DeleteTransaction(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<Transaction> { Status = false });

        var result = await transactionService.DeleteTransaction(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchTransactions()
    {
        transactionRepository.Setup(x => x.SearchTransactions(It.IsAny<string>()))
            .Returns([]);

        var result = transactionService.SearchTransactions("search");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionDTO>>(result);
    }

    [Fact]
    public void GetTransactionsByAttributes()
    {
        transactionRepository.Setup(x => x.GetTransactionsByAttributes(It.IsAny<Transaction>()))
            .Returns([]);

        var result = transactionService.GetTransactionsByAttributes(new TransactionDTO { });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionDTO>>(result);
    }
}