namespace MoneyTracker.Domain.Interfaces;

public interface ICacheProvider
{
    /// <summary>
    /// Check if a key exists in the cache
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(string key);

    /// <summary>
    /// Set a value in the cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiration"></param>
    /// <returns></returns>
    Task<bool> SetAsync<T>(string key, T? value, TimeSpan? expiration = null);

    /// <summary>
    /// Get a value from the cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Remove a value from the cache
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<bool> RemoveAsync(string key);
}