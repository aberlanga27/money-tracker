namespace MoneyTracker.Test.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyTracker.API.Controllers;
using MoneyTracker.API.Validator;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Interfaces;
using Moq;
using Xunit;

public class TransactionTypeControllerTest
{
    private readonly TransactionTypeController transactionTypeController;

    public TransactionTypeControllerTest()
    {
        transactionTypeController = new TransactionTypeController(
            new Mock<ILogger<TransactionTypeController>>().Object,
            new Mock<ILocalizationProvider>().Object,
            new Mock<ITransactionTypeService>().Object,
            new TransactionTypeValidator()
        );
    }

    [Fact]
    public async Task GetAll()
    {
        var result = await transactionTypeController.GetAll(null, null);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById()
    {
        var result = await transactionTypeController.GetById(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_InvalidId()
    {
        var result = await transactionTypeController.GetById(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create()
    {
        var result = await transactionTypeController.Create(new TransactionTypeDTO
        {
            TransactionTypeId = 1,
            TransactionTypeName = "test",
            TransactionTypeDescription = "test",
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_InvalidData()
    {
        var result = await transactionTypeController.Create(new TransactionTypeDTO
        {
            TransactionTypeId = -1,
            TransactionTypeName = null!,
            TransactionTypeDescription = null!,
            // CTX: invalid-test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update()
    {
        var result = await transactionTypeController.Update(new TransactionTypeDTO
        {
            TransactionTypeId = 1,
            TransactionTypeName = "test",
            TransactionTypeDescription = "test",
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_InvalidData()
    {
        var result = await transactionTypeController.Update(new TransactionTypeDTO { TransactionTypeId = -1 });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete()
    {
        var result = await transactionTypeController.Delete(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_InvalidId()
    {
        var result = await transactionTypeController.Delete(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Search()
    {
        var result = transactionTypeController.Search("transactionType");

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Search_InvalidQuery()
    {
        var result = transactionTypeController.Search(null!);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void GetByAttributes()
    {
        var result = transactionTypeController.GetByAttributes(new TransactionTypeDTO
        {
            TransactionTypeId = 1
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
}