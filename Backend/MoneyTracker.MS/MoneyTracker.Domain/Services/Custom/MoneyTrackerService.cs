namespace MoneyTracker.Domain.Services;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;

public class MoneyTrackerService(
    MoneyTrackerSettings appSettings,
    ILocalizationProvider translator,
    IMoneyTrackerRepository moneyTrackerRepository
) : IMoneyTrackerService
{
    private readonly MoneyTrackerSettings appSettings = Guard.Against.Null(appSettings);
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly IMoneyTrackerRepository moneyTrackerRepository = Guard.Against.Null(moneyTrackerRepository);

    public async Task<ValueResponse<string>> HealthCheckup()
    {
        var runningInDocker = appSettings.Environment?.RunningInContainer == true && appSettings.Environment?.DockerConnString != null;
        var serverDateTime = await moneyTrackerRepository.HealthCheckup();

        var message = runningInDocker
            ? translator.T("Running in Docker")
            : translator.T("Running in Localhost");

        if (serverDateTime == null)
            return new ValueResponse<string>
            {
                Status = false,
                Message = $"{message}, ${translator.T("but database is not connected")}"
            };

        return new ValueResponse<string>
        {
            Status = true,
            Message = translator.T("Echo"),
            Response = $"{message}, {translator.T("server date time is")} {serverDateTime}"
        };
    }
}