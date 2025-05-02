namespace MoneyTracker.Domain.Interfaces;

public interface IRepository<T> where T : class, new()
{
    /// <summary>
    /// Get all entities
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<int> AddAsync(T entity);

    /// <summary>
    /// Get all entities
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task<int> AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Get all entities
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(T entity);

    /// <summary>
    /// Get all entities
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(T entity);
}