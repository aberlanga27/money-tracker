namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;

public interface IExampleRepository
{
    /// <summary>
    /// Get the count of examples
    /// </summary>
    /// <returns></returns>
    Task<int> GetCount();

    /// <summary>
    /// Get all examples
    /// </summary>
    /// <returns></returns>
    IEnumerable<Example> GetAllExamples();

    /// <summary>
    /// Get all examples with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    IEnumerable<Example> GetAllExamples(int pageSize, int offsetSize);

    /// <summary>
    /// Get an example by its ID
    /// </summary>
    /// <param name="exampleId"></param>
    /// <returns></returns>
    Task<Example?> GetExampleById(int exampleId);

    /// <summary>
    /// Create an example
    /// </summary>
    /// <param name="example"></param>
    /// <returns></returns>
    Task<ValueResponse<Example>> CreateExample(Example example);

    /// <summary>
    /// Update an example
    /// </summary>
    /// <param name="example"></param>
    /// <returns></returns>
    Task<ValueResponse<Example>> UpdateExample(Example example);

    /// <summary>
    /// Delete an example
    /// </summary>
    /// <param name="exampleId"></param>
    /// <returns></returns>
    Task<ValueResponse<Example>> DeleteExample(int exampleId);

    /// <summary>
    /// Search examples by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<Example> SearchExamples(string search);

    /// <summary>
    /// Get examples by attributes
    /// </summary>
    /// <param name="example"></param>
    /// <returns></returns>
    IEnumerable<Example> GetExamplesByAttributes(Example example);
}