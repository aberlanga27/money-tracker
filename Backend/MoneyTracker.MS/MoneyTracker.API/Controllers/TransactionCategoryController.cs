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
/// Controller for the TransactionCategorys.
/// </summary>
[ApiController]
[Route($"{Constants.BasePath}/[controller]")]
// [Authorize]
// [ApiExplorerSettings(IgnoreApi = true)]
public class TransactionCategoryController(
    ILogger<TransactionCategoryController> logger,
    ILocalizationProvider translator,
    ITransactionCategoryService transactionCategoryService,
    IValidator<TransactionCategoryDTO> validator
) : ControllerBase
{
    private readonly ILogger<TransactionCategoryController> logger = Guard.Against.Null(logger);
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly ITransactionCategoryService transactionCategoryService = Guard.Against.Null(transactionCategoryService);
    private readonly IValidator<TransactionCategoryDTO> validator = Guard.Against.Null(validator);

    /// <summary>
    /// Gets all the TransactionCategorys.
    /// </summary>
    /// <param name="pageSize">The maximum number of TransactionCategorys to return.</param>
    /// <param name="offsetSize">The number of TransactionCategorys to skip.</param>
    /// <returns>A list of TransactionCategorys.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<IEnumerable<TransactionCategoryDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int? pageSize, int? offsetSize)
    {
        logger.LogDebug("Getting all transactionCategorys with pageSize {PageSize} and offsetSize {OffsetSize}", pageSize, offsetSize);

        var transactionCategorys = await transactionCategoryService.GetAllTransactionCategorys(pageSize, offsetSize);
        return Ok(transactionCategorys);
    }

    /// <summary>
    /// Gets an TransactionCategory by its ID.
    /// </summary>
    /// <param name="transactionCategoryId">The ID of the TransactionCategory.</param>
    /// <returns>An TransactionCategory.</returns>
    [HttpGet("{transactionCategoryId}")]
    [ProducesResponseType(typeof(ValueResponse<TransactionCategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int transactionCategoryId)
    {
        logger.LogDebug("Getting transactionCategory by id {TransactionCategoryId}", transactionCategoryId);

        if (transactionCategoryId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var transactionCategory = await transactionCategoryService.GetTransactionCategoryById(transactionCategoryId);
        return Ok(transactionCategory);
    }

    /// <summary>
    /// Creates a new TransactionCategory.
    /// </summary>
    /// <param name="transactionCategoryDTO">The TransactionCategory to create.</param>
    /// <returns>A response with the ID of the created TransactionCategory.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ValueResponse<TransactionCategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(TransactionCategoryDTO transactionCategoryDTO)
    {
        logger.LogDebug("Creating transactionCategory {TransactionCategoryDTO}", JsonConvert.SerializeObject(transactionCategoryDTO));

        var validation = await validator.ValidateAsync(transactionCategoryDTO, options => options.IncludeRuleSets(Constants.InsertRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await transactionCategoryService.CreateTransactionCategory(transactionCategoryDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Updates an TransactionCategory.
    /// </summary>
    /// <param name="transactionCategoryDTO">The TransactionCategory to update.</param>
    /// <returns>A response with the ID of the updated TransactionCategory.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(ValueResponse<TransactionCategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(TransactionCategoryDTO transactionCategoryDTO)
    {
        logger.LogDebug("Updating transactionCategory {TransactionCategoryDTO}", JsonConvert.SerializeObject(transactionCategoryDTO));

        var validation = await validator.ValidateAsync(transactionCategoryDTO, options => options.IncludeRuleSets(Constants.UpdateRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await transactionCategoryService.UpdateTransactionCategory(transactionCategoryDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Deletes an TransactionCategory.
    /// </summary>
    /// <param name="transactionCategoryId">The ID of the TransactionCategory to delete.</param>
    /// <returns>A response with the ID of the deleted TransactionCategory.</returns>
    [HttpDelete("{transactionCategoryId}")]
    [ProducesResponseType(typeof(ValueResponse<TransactionCategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int transactionCategoryId)
    {
        logger.LogDebug("Deleting transactionCategory by id {TransactionCategoryId}", transactionCategoryId);

        if (transactionCategoryId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var transactionResponse = await transactionCategoryService.DeleteTransactionCategory(transactionCategoryId);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Searches for TransactionCategorys by a string.
    /// </summary>
    /// <param name="search">The string to search for.</param>
    /// <returns>A list of TransactionCategorys.</returns>
    [HttpGet("Search")]
    [ProducesResponseType(typeof(IEnumerable<TransactionCategoryDTO>), StatusCodes.Status200OK)]
    public IActionResult Search(string search)
    {
        logger.LogDebug("Searching transactionCategorys by {Search}", search);

        if (string.IsNullOrWhiteSpace(search))
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid search, it can't be null or empty") });

        var transactionCategorys = transactionCategoryService.SearchTransactionCategorys(search);
        return Ok(transactionCategorys);
    }

    /// <summary>
    /// Gets an TransactionCategory by its attributes.
    /// </summary>
    /// <param name="transactionCategoryDTO">The attributes of the TransactionCategory.</param>
    /// <returns>An TransactionCategory.</returns>
    [HttpPost("Find")]
    [ProducesResponseType(typeof(IEnumerable<TransactionCategoryDTO>), StatusCodes.Status200OK)]
    public IActionResult GetByAttributes(TransactionCategoryDTO transactionCategoryDTO)
    {
        logger.LogDebug("Getting transactionCategory by attributes {TransactionCategoryDTO}", JsonConvert.SerializeObject(transactionCategoryDTO));

        var transactionCategorys = transactionCategoryService.GetTransactionCategorysByAttributes(transactionCategoryDTO);
        return Ok(transactionCategorys);
    }
}