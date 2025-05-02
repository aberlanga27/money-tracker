namespace MoneyTracker.Infrastructure.Providers;

using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

[ExcludeFromCodeCoverage]
public class RedisCacheProvider(
    MoneyTrackerSettings appSettings,
    IConnectionMultiplexer redisConnection
) : ICacheProvider
{
    private readonly MoneyTrackerSettings appSettings = Guard.Against.Null(appSettings);
    private readonly IDatabase redis = Guard.Against.Null(redisConnection?.GetDatabase());

    public async Task<bool> ExistsAsync(string key)
    {
        return await redis.KeyExistsAsync(key);
    }

    public async Task<bool> SetAsync<T>(string key, T? value, TimeSpan? expiration = null)
    {
        if (string.IsNullOrWhiteSpace(key) || value is null)
            return false;

        if (expiration == null)
            expiration = TimeSpan.FromMinutes(appSettings.Cache.ExpirationInMinutes);

        var serializedValue = JsonConvert.SerializeObject(value);
        await redis.StringSetAsync(key, serializedValue, expiration);
        return true;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return default;

        var serializedValue = await redis.StringGetAsync(key);

        return serializedValue.HasValue
            ? JsonConvert.DeserializeObject<T>(serializedValue!)
            : default;
    }

    public async Task<bool> RemoveAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        return await redis.KeyDeleteAsync(key);
    }
}