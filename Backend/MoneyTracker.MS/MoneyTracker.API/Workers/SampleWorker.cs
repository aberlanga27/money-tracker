
namespace MoneyTracker.API.Workers;

using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;

/// <summary>
/// SampleWorker class.
/// </summary>
/// <param name="appSettings"></param>
/// <param name="logger"></param>
/// <param name="serviceScopeFactory"></param>
[ExcludeFromCodeCoverage]
public class SampleWorker(
    MoneyTrackerSettings appSettings,
    ILogger<SampleWorker> logger,
    IServiceScopeFactory serviceScopeFactory
) : IHostedService, IDisposable
{
    private readonly ILogger<SampleWorker> logger = Guard.Against.Null(logger);
    private readonly IServiceScopeFactory serviceScopeFactory = Guard.Against.Null(serviceScopeFactory);

    private readonly int interval = appSettings.Workers.SampleInterval;
    private Timer? timer;

    /// <summary>
    /// Disposes the SampleWorker.
    /// </summary>
    public void Dispose()
    {
        timer?.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Starts the SampleWorker.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("SampleWorker running.");
        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(interval));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the SampleWorker.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("SampleWorker is stopping.");
        timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
    private async void DoWork(object? state)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var exampleService = scope.ServiceProvider.GetRequiredService<IExampleService>();

        try
        {
            await DoWorkLogic(exampleService);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while executing the SampleWorker.");
        }
    }

    private async Task DoWorkLogic(IExampleService exampleService)
    {
        logger.LogDebug("SampleWorker is working.");
        await Task.Delay(1);
    }
}