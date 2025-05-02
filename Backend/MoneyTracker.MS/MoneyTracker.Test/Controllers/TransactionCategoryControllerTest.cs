namespace MoneyTracker.Test.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyTracker.API.Controllers;
using MoneyTracker.API.Validator;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Interfaces;
using Moq;
using Xunit;

public class TransactionCategoryControllerTest
{
    private readonly TransactionCategoryController transactionCategoryController;

    public TransactionCategoryControllerTest()
    {
        transactionCategoryController = new TransactionCategoryController(
            new Mock<ILogger<TransactionCategoryController>>().Object,
            new Mock<ILocalizationProvider>().Object,
            new Mock<ITransactionCategoryService>().Object,
            new TransactionCategoryValidator()
        );
    }

    [Fact]
    public async Task GetAll()
    {
        var result = await transactionCategoryController.GetAll(null, null);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById()
    {
        var result = await transactionCategoryController.GetById(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_InvalidId()
    {
        var result = await transactionCategoryController.GetById(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create()
    {
        var result = await transactionCategoryController.Create(new TransactionCategoryDTO
        {
            TransactionCategoryId = 1,
            TransactionCategoryName = "test",
            TransactionCategoryDescription = "test",
            TransactionCategoryIcon = "test",
            TransactionCategoryColor = "test",
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_InvalidData()
    {
        var result = await transactionCategoryController.Create(new TransactionCategoryDTO
        {
            TransactionCategoryId = -1,
            TransactionCategoryName = null!,
            TransactionCategoryDescription = null!,
            TransactionCategoryIcon = null!,
            TransactionCategoryColor = null!,
            // CTX: invalid-test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update()
    {
        var result = await transactionCategoryController.Update(new TransactionCategoryDTO
        {
            TransactionCategoryId = 1,
            TransactionCategoryName = "test",
            TransactionCategoryDescription = "test",
            TransactionCategoryIcon = "test",
            TransactionCategoryColor = "test",
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_InvalidData()
    {
        var result = await transactionCategoryController.Update(new TransactionCategoryDTO { TransactionCategoryId = -1 });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete()
    {
        var result = await transactionCategoryController.Delete(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_InvalidId()
    {
        var result = await transactionCategoryController.Delete(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Search()
    {
        var result = transactionCategoryController.Search("transactionCategory");

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Search_InvalidQuery()
    {
        var result = transactionCategoryController.Search(null!);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void GetByAttributes()
    {
        var result = transactionCategoryController.GetByAttributes(new TransactionCategoryDTO
        {
            TransactionCategoryId = 1
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
}