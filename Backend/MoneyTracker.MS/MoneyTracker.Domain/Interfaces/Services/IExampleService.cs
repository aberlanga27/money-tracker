namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;

public interface IExampleService
{
    /// <summary>
    /// Get all examples
    /// </summary>
    /// <returns></returns>
    IEnumerable<ExampleDTO> GetAllExamples();

    /// <summary>
    /// Get all examples with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    Task<PaginationResponse<IEnumerable<ExampleDTO>>> GetAllExamples(int? pageSize, int? offsetSize);

    /// <summary>
    /// Get an example by its ID
    /// </summary>
    /// <param name="exampleId"></param>
    /// <returns></returns>
    Task<ValueResponse<ExampleDTO>> GetExampleById(int exampleId);

    /// <summary>
    /// Create an example
    /// </summary>
    /// <param name="exampleDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<ExampleDTO>> CreateExample(ExampleDTO exampleDTO);

    /// <summary>
    /// Update an example
    /// </summary>
    /// <param name="exampleDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<ExampleDTO>> UpdateExample(ExampleDTO exampleDTO);

    /// <summary>
    /// Delete an example
    /// </summary>
    /// <param name="exampleId"></param>
    /// <returns></returns>
    Task<ValueResponse<ExampleDTO>> DeleteExample(int exampleId);

    /// <summary>
    /// Search examples by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<ExampleDTO> SearchExamples(string search);

    /// <summary>
    /// Get examples by attributes
    /// </summary>
    /// <param name="exampleDTO"></param>
    /// <returns></returns>
    IEnumerable<ExampleDTO> GetExamplesByAttributes(ExampleDTO exampleDTO);
}