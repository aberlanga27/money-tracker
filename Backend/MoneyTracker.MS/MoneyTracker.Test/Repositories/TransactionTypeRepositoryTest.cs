namespace MoneyTracker.Test.Repositories;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Infrastructure.Repositories;
using MoneyTracker.Test.Utils;
using Moq;
using Xunit;

public class TransactionTypeRepositoryTest
{
    private readonly Mock<MoneyTrackerContext> mockContext;
    private readonly TransactionTypeRepository transactionTypeRepository;

    public TransactionTypeRepositoryTest()
    {
        mockContext = DbContextMockTools.GetMockedContext();
        transactionTypeRepository = new TransactionTypeRepository(mockContext.Object, new Mock<ILocalizationProvider>().Object);
    }

    [Fact]
    public async Task GetCount()
    {
        var result = await transactionTypeRepository.GetCount();

        Assert.IsType<int>(result);
    }

    [Fact]
    public void GetAllTransactionTypes()
    {
        var result = transactionTypeRepository.GetAllTransactionTypes();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionType>>(result);
    }

    [Fact]
    public void GetAllTransactionTypes_WithPagination()
    {
        var result = transactionTypeRepository.GetAllTransactionTypes(0, 0);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionType>>(result);
    }

    [Fact]
    public async Task GetTransactionTypeById()
    {
        var result = await transactionTypeRepository.GetTransactionTypeById(1);

        Assert.NotNull(result);
        Assert.IsType<TransactionType>(result);
    }

    [Fact]
    public async Task CreateTransactionType()
    {
        var transactionType = new TransactionType()
        {
            TransactionTypeId = 0,
            TransactionTypeName = "test_new",
            TransactionTypeDescription = "test_new",
            // CTX: new-test-attribute, do not remove this line
        };

        var result = await transactionTypeRepository.CreateTransactionType(transactionType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.True(result.Status);
        Assert.Equal(transactionType, result.Response);
    }

    [Fact]
    public async Task CreateTransactionType_InvalidUK()
    {
        var transactionType = new TransactionType()
        {
            TransactionTypeId = 0,
            TransactionTypeName = "test",
            TransactionTypeDescription = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionTypeRepository.CreateTransactionType(transactionType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateTransactionType_InvalidFK()
    {
        var transactionType = new TransactionType()
        {
            TransactionTypeId = 0,
            TransactionTypeName = null!,
            TransactionTypeDescription = null!,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await transactionTypeRepository.CreateTransactionType(transactionType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateTransactionType_Update()
    {
        var transactionType = new TransactionType()
        {
            TransactionTypeId = 1,
            TransactionTypeName = "test",
            TransactionTypeDescription = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionTypeRepository.CreateTransactionType(transactionType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.True(result.Status);
        Assert.Equal(transactionType, result.Response);
    }

    [Fact]
    public async Task UpdateTransactionType()
    {
        var transactionType = new TransactionType()
        {
            TransactionTypeId = 1,
            TransactionTypeName = "test",
            TransactionTypeDescription = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionTypeRepository.UpdateTransactionType(transactionType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.True(result.Status);
        Assert.Equal(transactionType, result.Response);
    }

    [Fact]
    public async Task UpdateTransactionType_InvalidUK()
    {
        var transactionType = new TransactionType()
        {
            TransactionTypeId = 2,
            TransactionTypeName = "test",
            TransactionTypeDescription = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionTypeRepository.UpdateTransactionType(transactionType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateTransactionType_InvalidFK()
    {
        var transactionType = new TransactionType()
        {
            TransactionTypeId = 2,
            TransactionTypeName = null!,
            TransactionTypeDescription = null!,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await transactionTypeRepository.UpdateTransactionType(transactionType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateTransactionType_NotFound()
    {
        var transactionType = new TransactionType()
        {
            TransactionTypeId = 3,
            TransactionTypeName = "test",
            TransactionTypeDescription = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionTypeRepository.UpdateTransactionType(transactionType);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteTransactionType()
    {
        var result = await transactionTypeRepository.DeleteTransactionType(2);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteTransactionType_InvalidFK()
    {
        var result = await transactionTypeRepository.DeleteTransactionType(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteTransactionType_NotFound()
    {
        var result = await transactionTypeRepository.DeleteTransactionType(3);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionType>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchTransactionTypes()
    {
        var result = transactionTypeRepository.SearchTransactionTypes("1");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionType>>(result);
    }

    [Fact]
    public void GetTransactionTypesByAttributes()
    {
        var result = transactionTypeRepository.GetTransactionTypesByAttributes(new TransactionType
        {
            TransactionTypeId = 1,
            TransactionTypeName = "test",
            TransactionTypeDescription = "test",
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionType>>(result);
    }
}