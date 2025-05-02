namespace MoneyTracker.Test.Repositories;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Infrastructure.Repositories;
using MoneyTracker.Test.Utils;
using Moq;
using Xunit;

public class TransactionRepositoryTest
{
    private readonly Mock<MoneyTrackerContext> mockContext;
    private readonly TransactionRepository transactionRepository;

    public TransactionRepositoryTest()
    {
        mockContext = DbContextMockTools.GetMockedContext();
        transactionRepository = new TransactionRepository(mockContext.Object, new Mock<ILocalizationProvider>().Object);
    }

    [Fact]
    public async Task GetCount()
    {
        var result = await transactionRepository.GetCount();

        Assert.IsType<int>(result);
    }

    [Fact]
    public void GetAllTransactions()
    {
        var result = transactionRepository.GetAllTransactions();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Transaction>>(result);
    }

    [Fact]
    public void GetAllTransactions_WithPagination()
    {
        var result = transactionRepository.GetAllTransactions(0, 0);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Transaction>>(result);
    }

    [Fact]
    public async Task GetTransactionById()
    {
        var result = await transactionRepository.GetTransactionById(1);

        Assert.NotNull(result);
        Assert.IsType<Transaction>(result);
    }

    [Fact]
    public async Task CreateTransaction()
    {
        var transaction = new Transaction()
        {
            TransactionId = 0,
            TransactionCategoryId = 2,
            TransactionTypeId = 2,
            BankId = 2,
            TransactionAmount = 2.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = "test_new",
            // CTX: new-test-attribute, do not remove this line
        };

        var result = await transactionRepository.CreateTransaction(transaction);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.True(result.Status);
        Assert.Equal(transaction, result.Response);
    }

    [Fact]
    public async Task CreateTransaction_InvalidUK()
    {
        var transaction = new Transaction()
        {
            TransactionId = 0,
            TransactionCategoryId = 1,
            TransactionTypeId = 1,
            BankId = 1,
            TransactionAmount = 1.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionRepository.CreateTransaction(transaction);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateTransaction_InvalidFK()
    {
        var transaction = new Transaction()
        {
            TransactionId = 0,
            TransactionCategoryId = -1,
            TransactionTypeId = -1,
            BankId = -1,
            TransactionAmount = -1.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = null!,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await transactionRepository.CreateTransaction(transaction);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateTransaction_Update()
    {
        var transaction = new Transaction()
        {
            TransactionId = 1,
            TransactionCategoryId = 1,
            TransactionTypeId = 1,
            BankId = 1,
            TransactionAmount = 1.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionRepository.CreateTransaction(transaction);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.True(result.Status);
        Assert.Equal(transaction, result.Response);
    }

    [Fact]
    public async Task UpdateTransaction()
    {
        var transaction = new Transaction()
        {
            TransactionId = 1,
            TransactionCategoryId = 1,
            TransactionTypeId = 1,
            BankId = 1,
            TransactionAmount = 1.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionRepository.UpdateTransaction(transaction);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.True(result.Status);
        Assert.Equal(transaction, result.Response);
    }

    [Fact]
    public async Task UpdateTransaction_InvalidUK()
    {
        var transaction = new Transaction()
        {
            TransactionId = 2,
            TransactionCategoryId = 1,
            TransactionTypeId = 1,
            BankId = 1,
            TransactionAmount = 1.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionRepository.UpdateTransaction(transaction);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateTransaction_InvalidFK()
    {
        var transaction = new Transaction()
        {
            TransactionId = 2,
            TransactionCategoryId = -1,
            TransactionTypeId = -1,
            BankId = -1,
            TransactionAmount = -1.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = null!,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await transactionRepository.UpdateTransaction(transaction);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateTransaction_NotFound()
    {
        var transaction = new Transaction()
        {
            TransactionId = 3,
            TransactionCategoryId = 1,
            TransactionTypeId = 1,
            BankId = 1,
            TransactionAmount = 1.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionRepository.UpdateTransaction(transaction);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteTransaction()
    {
        var result = await transactionRepository.DeleteTransaction(2);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteTransaction_InvalidFK()
    {
        var result = await transactionRepository.DeleteTransaction(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteTransaction_NotFound()
    {
        var result = await transactionRepository.DeleteTransaction(3);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Transaction>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchTransactions()
    {
        var result = transactionRepository.SearchTransactions("1");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Transaction>>(result);
    }

    [Fact]
    public void GetTransactionsByAttributes()
    {
        var result = transactionRepository.GetTransactionsByAttributes(new Transaction
        {
            TransactionId = 1,
            TransactionCategoryId = 1,
            TransactionTypeId = 1,
            BankId = 1,
            TransactionAmount = 1.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = "test",
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Transaction>>(result);
    }
}