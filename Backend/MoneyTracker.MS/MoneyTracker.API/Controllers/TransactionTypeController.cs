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
/// Controller for the TransactionTypes.
/// </summary>
[ApiController]
[Route($"{Constants.BasePath}/[controller]")]
// [Authorize]
// [ApiExplorerSettings(IgnoreApi = true)]
public class TransactionTypeController(
    ILogger<TransactionTypeController> logger,
    ILocalizationProvider translator,
    ITransactionTypeService transactionTypeService,
    IValidator<TransactionTypeDTO> validator
) : ControllerBase
{
    private readonly ILogger<TransactionTypeController> logger = Guard.Against.Null(logger);
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly ITransactionTypeService transactionTypeService = Guard.Against.Null(transactionTypeService);
    private readonly IValidator<TransactionTypeDTO> validator = Guard.Against.Null(validator);

    /// <summary>
    /// Gets all the TransactionTypes.
    /// </summary>
    /// <param name="pageSize">The maximum number of TransactionTypes to return.</param>
    /// <param name="offsetSize">The number of TransactionTypes to skip.</param>
    /// <returns>A list of TransactionTypes.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<IEnumerable<TransactionTypeDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int? pageSize, int? offsetSize)
    {
        logger.LogDebug("Getting all transactionTypes with pageSize {PageSize} and offsetSize {OffsetSize}", pageSize, offsetSize);

        var transactionTypes = await transactionTypeService.GetAllTransactionTypes(pageSize, offsetSize);
        return Ok(transactionTypes);
    }

    /// <summary>
    /// Gets an TransactionType by its ID.
    /// </summary>
    /// <param name="transactionTypeId">The ID of the TransactionType.</param>
    /// <returns>An TransactionType.</returns>
    [HttpGet("{transactionTypeId}")]
    [ProducesResponseType(typeof(ValueResponse<TransactionTypeDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int transactionTypeId)
    {
        logger.LogDebug("Getting transactionType by id {TransactionTypeId}", transactionTypeId);

        if (transactionTypeId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var transactionType = await transactionTypeService.GetTransactionTypeById(transactionTypeId);
        return Ok(transactionType);
    }

    /// <summary>
    /// Creates a new TransactionType.
    /// </summary>
    /// <param name="transactionTypeDTO">The TransactionType to create.</param>
    /// <returns>A response with the ID of the created TransactionType.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ValueResponse<TransactionTypeDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(TransactionTypeDTO transactionTypeDTO)
    {
        logger.LogDebug("Creating transactionType {TransactionTypeDTO}", JsonConvert.SerializeObject(transactionTypeDTO));

        var validation = await validator.ValidateAsync(transactionTypeDTO, options => options.IncludeRuleSets(Constants.InsertRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await transactionTypeService.CreateTransactionType(transactionTypeDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Updates an TransactionType.
    /// </summary>
    /// <param name="transactionTypeDTO">The TransactionType to update.</param>
    /// <returns>A response with the ID of the updated TransactionType.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(ValueResponse<TransactionTypeDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(TransactionTypeDTO transactionTypeDTO)
    {
        logger.LogDebug("Updating transactionType {TransactionTypeDTO}", JsonConvert.SerializeObject(transactionTypeDTO));

        var validation = await validator.ValidateAsync(transactionTypeDTO, options => options.IncludeRuleSets(Constants.UpdateRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await transactionTypeService.UpdateTransactionType(transactionTypeDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Deletes an TransactionType.
    /// </summary>
    /// <param name="transactionTypeId">The ID of the TransactionType to delete.</param>
    /// <returns>A response with the ID of the deleted TransactionType.</returns>
    [HttpDelete("{transactionTypeId}")]
    [ProducesResponseType(typeof(ValueResponse<TransactionTypeDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int transactionTypeId)
    {
        logger.LogDebug("Deleting transactionType by id {TransactionTypeId}", transactionTypeId);

        if (transactionTypeId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var transactionResponse = await transactionTypeService.DeleteTransactionType(transactionTypeId);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Searches for TransactionTypes by a string.
    /// </summary>
    /// <param name="search">The string to search for.</param>
    /// <returns>A list of TransactionTypes.</returns>
    [HttpGet("Search")]
    [ProducesResponseType(typeof(IEnumerable<TransactionTypeDTO>), StatusCodes.Status200OK)]
    public IActionResult Search(string search)
    {
        logger.LogDebug("Searching transactionTypes by {Search}", search);

        if (string.IsNullOrWhiteSpace(search))
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid search, it can't be null or empty") });

        var transactionTypes = transactionTypeService.SearchTransactionTypes(search);
        return Ok(transactionTypes);
    }

    /// <summary>
    /// Gets an TransactionType by its attributes.
    /// </summary>
    /// <param name="transactionTypeDTO">The attributes of the TransactionType.</param>
    /// <returns>An TransactionType.</returns>
    [HttpPost("Find")]
    [ProducesResponseType(typeof(IEnumerable<TransactionTypeDTO>), StatusCodes.Status200OK)]
    public IActionResult GetByAttributes(TransactionTypeDTO transactionTypeDTO)
    {
        logger.LogDebug("Getting transactionType by attributes {TransactionTypeDTO}", JsonConvert.SerializeObject(transactionTypeDTO));

        var transactionTypes = transactionTypeService.GetTransactionTypesByAttributes(transactionTypeDTO);
        return Ok(transactionTypes);
    }
}