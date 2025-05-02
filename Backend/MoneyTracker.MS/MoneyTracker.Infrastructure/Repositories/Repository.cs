namespace MoneyTracker.Infrastructure.Repositories;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.Interfaces;

public class Repository<T>(
    MoneyTrackerContext context
) : IRepository<T> where T : class, new()
{
    public readonly MoneyTrackerContext context = Guard.Against.Null(context);

    public async Task<int> AddAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await context.AddAsync(entity);
        var result = await context.SaveChangesAsync();
        return result;
    }

    public async Task<int> AddRangeAsync(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await context.AddRangeAsync(entities);
        var result = await context.SaveChangesAsync();
        return result;
    }

    public async Task<int> UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        context.Update(entity);
        var result = await context.SaveChangesAsync();
        return result;
    }

    public async Task<int> DeleteAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        context.Remove(entity);
        var result = await context.SaveChangesAsync();
        return result;
    }
}