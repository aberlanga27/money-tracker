namespace MoneyTracker.Test.Services;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Services;
using Moq;
using Xunit;

public class MoneyTrackerServiceTest
{
    private readonly Mock<MoneyTrackerSettings> appSettings = new();
    private readonly Mock<IMoneyTrackerRepository> moneyTrackerRepository = new();
    private readonly MoneyTrackerService moneyTrackerService;

    public MoneyTrackerServiceTest()
    {
        moneyTrackerService = new MoneyTrackerService(
            appSettings.Object,
            new Mock<ILocalizationProvider>().Object,
            moneyTrackerRepository.Object
        );
    }

    [Fact]
    public async Task HealthCheckup()
    {
        moneyTrackerRepository.Setup(x => x.HealthCheckup())
            .ReturnsAsync(DateTime.Now);

        var result = await moneyTrackerService.HealthCheckup();

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<string>>(result);
    }

    [Fact]
    public async Task HealthCheckup_NoDbConnection()
    {
        moneyTrackerRepository.Setup(x => x.HealthCheckup())
            .ReturnsAsync((DateTime?)null);

        var result = await moneyTrackerService.HealthCheckup();

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<string>>(result);
    }

    [Fact]
    public async Task HealthCheckup_DockerConnection()
    {
        appSettings.Object.Environment = new MoneyTrackerSettings.EnvironmentModel
        {
            RunningInContainer = true,
            DockerConnString = "test"
        };

        moneyTrackerRepository.Setup(x => x.HealthCheckup())
            .ReturnsAsync((DateTime?)null);

        var result = await moneyTrackerService.HealthCheckup();

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<string>>(result);
    }
}