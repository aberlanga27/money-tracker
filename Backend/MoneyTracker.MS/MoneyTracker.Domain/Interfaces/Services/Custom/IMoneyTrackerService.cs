namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;

public interface IMoneyTrackerService
{
    /// <summary>
    /// Check if the API is healthy
    /// </summary>
    /// <returns></returns>
    Task<ValueResponse<string>> HealthCheckup();
}