namespace MoneyTracker.Test.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyTracker.API.Controllers;
using MoneyTracker.API.Validator;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Interfaces;
using Moq;
using Xunit;

public class BudgetTypeControllerTest
{
    private readonly BudgetTypeController budgetTypeController;

    public BudgetTypeControllerTest()
    {
        budgetTypeController = new BudgetTypeController(
            new Mock<ILogger<BudgetTypeController>>().Object,
            new Mock<ILocalizationProvider>().Object,
            new Mock<IBudgetTypeService>().Object,
            new BudgetTypeValidator()
        );
    }

    [Fact]
    public async Task GetAll()
    {
        var result = await budgetTypeController.GetAll(null, null);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById()
    {
        var result = await budgetTypeController.GetById(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_InvalidId()
    {
        var result = await budgetTypeController.GetById(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create()
    {
        var result = await budgetTypeController.Create(new BudgetTypeDTO
        {
            BudgetTypeId = 1,
            BudgetTypeName = "test",
            BudgetTypeDays = 1,
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_InvalidData()
    {
        var result = await budgetTypeController.Create(new BudgetTypeDTO
        {
            BudgetTypeId = -1,
            BudgetTypeName = null!,
            BudgetTypeDays = -1,
            // CTX: invalid-test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update()
    {
        var result = await budgetTypeController.Update(new BudgetTypeDTO
        {
            BudgetTypeId = 1,
            BudgetTypeName = "test",
            BudgetTypeDays = 1,
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_InvalidData()
    {
        var result = await budgetTypeController.Update(new BudgetTypeDTO { BudgetTypeId = -1 });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete()
    {
        var result = await budgetTypeController.Delete(1);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_InvalidId()
    {
        var result = await budgetTypeController.Delete(-1);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Search()
    {
        var result = budgetTypeController.Search("budgetType");

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Search_InvalidQuery()
    {
        var result = budgetTypeController.Search(null!);

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void GetByAttributes()
    {
        var result = budgetTypeController.GetByAttributes(new BudgetTypeDTO
        {
            BudgetTypeId = 1
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
}