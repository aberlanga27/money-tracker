namespace MoneyTracker.Test.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyTracker.API.Controllers;
using MoneyTracker.API.Validator;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Interfaces;
using Moq;
using Xunit;

public class BudgetControllerTest
{
    private readonly BudgetController budgetController;

    public BudgetControllerTest()
    {
        budgetController = new BudgetController(
            new Mock<ILogger<BudgetController>>().Object,
            new Mock<ILocalizationProvider>().Object,
            new Mock<IBudgetService>().Object,
            new BudgetValidator()
        );
    }

    [Fact]
    public async Task GetAll()
    {
        var result = await budgetController.GetAll(null, null);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById()
    {
        var result = await budgetController.GetById(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_InvalidId()
    {
        var result = await budgetController.GetById(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create()
    {
        var result = await budgetController.Create(new BudgetDTO
        {
            BudgetId = 1,
            TransactionCategoryId = 1,
            BudgetTypeId = 1,
            BudgetAmount = 1.0m,
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_InvalidData()
    {
        var result = await budgetController.Create(new BudgetDTO
        {
            BudgetId = -1,
            TransactionCategoryId = -1,
            BudgetTypeId = -1,
            BudgetAmount = -1.0m,
            // CTX: invalid-test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update()
    {
        var result = await budgetController.Update(new BudgetDTO
        {
            BudgetId = 1,
            TransactionCategoryId = 1,
            BudgetTypeId = 1,
            BudgetAmount = 1.0m,
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_InvalidData()
    {
        var result = await budgetController.Update(new BudgetDTO { BudgetId = -1 });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete()
    {
        var result = await budgetController.Delete(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_InvalidId()
    {
        var result = await budgetController.Delete(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Search()
    {
        var result = budgetController.Search("budget");

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Search_InvalidQuery()
    {
        var result = budgetController.Search(null!);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void GetByAttributes()
    {
        var result = budgetController.GetByAttributes(new BudgetDTO
        {
            BudgetId = 1
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
}