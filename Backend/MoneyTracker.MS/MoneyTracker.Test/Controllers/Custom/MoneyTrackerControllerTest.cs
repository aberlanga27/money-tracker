namespace MoneyTracker.Test.Controllers;

using Microsoft.AspNetCore.Mvc;
using MoneyTracker.API.Controllers;
using MoneyTracker.Domain.Interfaces;
using Moq;
using Xunit;

public class MoneyTrackerControllerTest
{
    private readonly MoneyTrackerController moneyTrackerController;

    public MoneyTrackerControllerTest()
    {
        moneyTrackerController = new MoneyTrackerController(
            new Mock<IMoneyTrackerService>().Object
        );
    }

    [Fact]
    public async Task HealthCheckup()
    {
        var result = await moneyTrackerController.HealthCheckup();

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
}