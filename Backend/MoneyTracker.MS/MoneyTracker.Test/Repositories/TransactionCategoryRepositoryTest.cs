namespace MoneyTracker.Test.Repositories;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Infrastructure.Repositories;
using MoneyTracker.Test.Utils;
using Moq;
using Xunit;

public class TransactionCategoryRepositoryTest
{
    private readonly Mock<MoneyTrackerContext> mockContext;
    private readonly TransactionCategoryRepository transactionCategoryRepository;

    public TransactionCategoryRepositoryTest()
    {
        mockContext = DbContextMockTools.GetMockedContext();
        transactionCategoryRepository = new TransactionCategoryRepository(mockContext.Object, new Mock<ILocalizationProvider>().Object);
    }

    [Fact]
    public async Task GetCount()
    {
        var result = await transactionCategoryRepository.GetCount();

        Assert.IsType<int>(result);
    }

    [Fact]
    public void GetAllTransactionCategorys()
    {
        var result = transactionCategoryRepository.GetAllTransactionCategorys();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionCategory>>(result);
    }

    [Fact]
    public void GetAllTransactionCategorys_WithPagination()
    {
        var result = transactionCategoryRepository.GetAllTransactionCategorys(0, 0);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionCategory>>(result);
    }

    [Fact]
    public async Task GetTransactionCategoryById()
    {
        var result = await transactionCategoryRepository.GetTransactionCategoryById(1);

        Assert.NotNull(result);
        Assert.IsType<TransactionCategory>(result);
    }

    [Fact]
    public async Task CreateTransactionCategory()
    {
        var transactionCategory = new TransactionCategory()
        {
            TransactionCategoryId = 0,
            TransactionCategoryName = "test_new",
            TransactionCategoryDescription = "test_new",
            TransactionCategoryIcon = "test_new",
            TransactionCategoryColor = "test_new",
            // CTX: new-test-attribute, do not remove this line
        };

        var result = await transactionCategoryRepository.CreateTransactionCategory(transactionCategory);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.True(result.Status);
        Assert.Equal(transactionCategory, result.Response);
    }

    [Fact]
    public async Task CreateTransactionCategory_InvalidUK()
    {
        var transactionCategory = new TransactionCategory()
        {
            TransactionCategoryId = 0,
            TransactionCategoryName = "test",
            TransactionCategoryDescription = "test",
            TransactionCategoryIcon = "test",
            TransactionCategoryColor = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionCategoryRepository.CreateTransactionCategory(transactionCategory);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateTransactionCategory_InvalidFK()
    {
        var transactionCategory = new TransactionCategory()
        {
            TransactionCategoryId = 0,
            TransactionCategoryName = null!,
            TransactionCategoryDescription = null!,
            TransactionCategoryIcon = null!,
            TransactionCategoryColor = null!,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await transactionCategoryRepository.CreateTransactionCategory(transactionCategory);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateTransactionCategory_Update()
    {
        var transactionCategory = new TransactionCategory()
        {
            TransactionCategoryId = 1,
            TransactionCategoryName = "test",
            TransactionCategoryDescription = "test",
            TransactionCategoryIcon = "test",
            TransactionCategoryColor = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionCategoryRepository.CreateTransactionCategory(transactionCategory);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.True(result.Status);
        Assert.Equal(transactionCategory, result.Response);
    }

    [Fact]
    public async Task UpdateTransactionCategory()
    {
        var transactionCategory = new TransactionCategory()
        {
            TransactionCategoryId = 1,
            TransactionCategoryName = "test",
            TransactionCategoryDescription = "test",
            TransactionCategoryIcon = "test",
            TransactionCategoryColor = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionCategoryRepository.UpdateTransactionCategory(transactionCategory);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.True(result.Status);
        Assert.Equal(transactionCategory, result.Response);
    }

    [Fact]
    public async Task UpdateTransactionCategory_InvalidUK()
    {
        var transactionCategory = new TransactionCategory()
        {
            TransactionCategoryId = 2,
            TransactionCategoryName = "test",
            TransactionCategoryDescription = "test",
            TransactionCategoryIcon = "test",
            TransactionCategoryColor = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionCategoryRepository.UpdateTransactionCategory(transactionCategory);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateTransactionCategory_InvalidFK()
    {
        var transactionCategory = new TransactionCategory()
        {
            TransactionCategoryId = 2,
            TransactionCategoryName = null!,
            TransactionCategoryDescription = null!,
            TransactionCategoryIcon = null!,
            TransactionCategoryColor = null!,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await transactionCategoryRepository.UpdateTransactionCategory(transactionCategory);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateTransactionCategory_NotFound()
    {
        var transactionCategory = new TransactionCategory()
        {
            TransactionCategoryId = 3,
            TransactionCategoryName = "test",
            TransactionCategoryDescription = "test",
            TransactionCategoryIcon = "test",
            TransactionCategoryColor = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await transactionCategoryRepository.UpdateTransactionCategory(transactionCategory);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteTransactionCategory()
    {
        var result = await transactionCategoryRepository.DeleteTransactionCategory(2);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteTransactionCategory_InvalidFK()
    {
        var result = await transactionCategoryRepository.DeleteTransactionCategory(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteTransactionCategory_NotFound()
    {
        var result = await transactionCategoryRepository.DeleteTransactionCategory(3);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<TransactionCategory>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchTransactionCategorys()
    {
        var result = transactionCategoryRepository.SearchTransactionCategorys("1");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionCategory>>(result);
    }

    [Fact]
    public void GetTransactionCategorysByAttributes()
    {
        var result = transactionCategoryRepository.GetTransactionCategorysByAttributes(new TransactionCategory
        {
            TransactionCategoryId = 1,
            TransactionCategoryName = "test",
            TransactionCategoryDescription = "test",
            TransactionCategoryIcon = "test",
            TransactionCategoryColor = "test",
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TransactionCategory>>(result);
    }
}