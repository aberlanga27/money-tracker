namespace MoneyTracker.Test.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyTracker.API.Controllers;
using MoneyTracker.API.Validator;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Interfaces;
using Moq;
using Xunit;

public class TransactionControllerTest
{
    private readonly TransactionController transactionController;

    public TransactionControllerTest()
    {
        transactionController = new TransactionController(
            new Mock<ILogger<TransactionController>>().Object,
            new Mock<ILocalizationProvider>().Object,
            new Mock<ITransactionService>().Object,
            new TransactionValidator()
        );
    }

    [Fact]
    public async Task GetAll()
    {
        var result = await transactionController.GetAll(null, null);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById()
    {
        var result = await transactionController.GetById(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_InvalidId()
    {
        var result = await transactionController.GetById(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create()
    {
        var result = await transactionController.Create(new TransactionDTO
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
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_InvalidData()
    {
        var result = await transactionController.Create(new TransactionDTO
        {
            TransactionId = -1,
            TransactionCategoryId = -1,
            TransactionTypeId = -1,
            BankId = -1,
            TransactionAmount = -1.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = null!,
            // CTX: invalid-test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update()
    {
        var result = await transactionController.Update(new TransactionDTO
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
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_InvalidData()
    {
        var result = await transactionController.Update(new TransactionDTO { TransactionId = -1 });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete()
    {
        var result = await transactionController.Delete(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_InvalidId()
    {
        var result = await transactionController.Delete(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Search()
    {
        var result = transactionController.Search("transaction");

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Search_InvalidQuery()
    {
        var result = transactionController.Search(null!);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void GetByAttributes()
    {
        var result = transactionController.GetByAttributes(new TransactionDTO
        {
            TransactionId = 1
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
}