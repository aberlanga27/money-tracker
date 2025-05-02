namespace MoneyTracker.Domain.Interfaces;

public interface IMoneyTrackerRepository
{
    /// <summary>
    /// Check if the DB of the API is healthy
    /// </summary>
    /// <returns></returns>
    Task<DateTime?> HealthCheckup();
}