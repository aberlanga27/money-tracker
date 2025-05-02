namespace MoneyTracker.API.Controllers;

using Ardalis.GuardClauses;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MoneyTracker.API.Common;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Interfaces;
using Newtonsoft.Json;

/// <summary>
/// Controller for the Examples.
/// </summary>
[ApiController]
[Route($"{Constants.BasePath}/[controller]")]
// [Authorize]
[ApiExplorerSettings(IgnoreApi = true)]
public class ExampleController(
    ILogger<ExampleController> logger,
    ILocalizationProvider translator,
    IExampleService exampleService,
    IValidator<ExampleDTO> validator
) : ControllerBase
{
    private readonly ILogger<ExampleController> logger = Guard.Against.Null(logger);
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly IExampleService exampleService = Guard.Against.Null(exampleService);
    private readonly IValidator<ExampleDTO> validator = Guard.Against.Null(validator);

    /// <summary>
    /// Gets all the Examples.
    /// </summary>
    /// <param name="pageSize">The maximum number of Examples to return.</param>
    /// <param name="offsetSize">The number of Examples to skip.</param>
    /// <returns>A list of Examples.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<IEnumerable<ExampleDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int? pageSize, int? offsetSize)
    {
        logger.LogDebug("Getting all examples with pageSize {PageSize} and offsetSize {OffsetSize}", pageSize, offsetSize);

        var examples = await exampleService.GetAllExamples(pageSize, offsetSize);
        return Ok(examples);
    }

    /// <summary>
    /// Gets an Example by its ID.
    /// </summary>
    /// <param name="exampleId">The ID of the Example.</param>
    /// <returns>An Example.</returns>
    [HttpGet("{exampleId}")]
    [ProducesResponseType(typeof(ValueResponse<ExampleDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int exampleId)
    {
        logger.LogDebug("Getting example by id {ExampleId}", exampleId);

        if (exampleId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var example = await exampleService.GetExampleById(exampleId);
        return Ok(example);
    }

    /// <summary>
    /// Creates a new Example.
    /// </summary>
    /// <param name="exampleDTO">The Example to create.</param>
    /// <returns>A response with the ID of the created Example.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ValueResponse<ExampleDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(ExampleDTO exampleDTO)
    {
        logger.LogDebug("Creating example {ExampleDTO}", JsonConvert.SerializeObject(exampleDTO));

        var validation = await validator.ValidateAsync(exampleDTO, options => options.IncludeRuleSets(Constants.InsertRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await exampleService.CreateExample(exampleDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Updates an Example.
    /// </summary>
    /// <param name="exampleDTO">The Example to update.</param>
    /// <returns>A response with the ID of the updated Example.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(ValueResponse<ExampleDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(ExampleDTO exampleDTO)
    {
        logger.LogDebug("Updating example {ExampleDTO}", JsonConvert.SerializeObject(exampleDTO));

        var validation = await validator.ValidateAsync(exampleDTO, options => options.IncludeRuleSets(Constants.UpdateRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await exampleService.UpdateExample(exampleDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Deletes an Example.
    /// </summary>
    /// <param name="exampleId">The ID of the Example to delete.</param>
    /// <returns>A response with the ID of the deleted Example.</returns>
    [HttpDelete("{exampleId}")]
    [ProducesResponseType(typeof(ValueResponse<ExampleDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int exampleId)
    {
        logger.LogDebug("Deleting example by id {ExampleId}", exampleId);

        if (exampleId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var transactionResponse = await exampleService.DeleteExample(exampleId);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Searches for Examples by a string.
    /// </summary>
    /// <param name="search">The string to search for.</param>
    /// <returns>A list of Examples.</returns>
    [HttpGet("Search")]
    [ProducesResponseType(typeof(IEnumerable<ExampleDTO>), StatusCodes.Status200OK)]
    public IActionResult Search(string search)
    {
        logger.LogDebug("Searching examples by {Search}", search);

        if (string.IsNullOrWhiteSpace(search))
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid search, it can't be null or empty") });

        var examples = exampleService.SearchExamples(search);
        return Ok(examples);
    }

    /// <summary>
    /// Gets an Example by its attributes.
    /// </summary>
    /// <param name="exampleDTO">The attributes of the Example.</param>
    /// <returns>An Example.</returns>
    [HttpPost("Find")]
    [ProducesResponseType(typeof(IEnumerable<ExampleDTO>), StatusCodes.Status200OK)]
    public IActionResult GetByAttributes(ExampleDTO exampleDTO)
    {
        logger.LogDebug("Getting example by attributes {ExampleDTO}", JsonConvert.SerializeObject(exampleDTO));

        var examples = exampleService.GetExamplesByAttributes(exampleDTO);
        return Ok(examples);
    }
}