namespace MoneyTracker.Infrastructure.Providers;

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Caching.Distributed;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using Newtonsoft.Json;

[ExcludeFromCodeCoverage]
public class LocalCacheProvider(
    MoneyTrackerSettings appSettings,
    IDistributedCache distributedCache
) : ICacheProvider
{
    private readonly MoneyTrackerSettings appSettings = Guard.Against.Null(appSettings);
    private readonly IDistributedCache cache = Guard.Against.Null(distributedCache);

    public async Task<bool> ExistsAsync(string key)
    {
        return await cache.GetStringAsync(key) != null;
    }

    public async Task<bool> SetAsync<T>(string key, T? value, TimeSpan? expiration = null)
    {
        if (string.IsNullOrWhiteSpace(key) || value is null)
            return false;

        if (expiration == null)
            expiration = TimeSpan.FromMinutes(appSettings.Cache.ExpirationInMinutes);

        var serializedValue = JsonConvert.SerializeObject(value);
        await cache.SetStringAsync(key, serializedValue, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        });
        return true;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return default;

        var serializedValue = await cache.GetStringAsync(key);

        return serializedValue != null
            ? JsonConvert.DeserializeObject<T>(serializedValue)
            : default;
    }

    public async Task<bool> RemoveAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        return await Task.Run(() =>
        {
            cache.Remove(key);
            return true;
        });
    }
}