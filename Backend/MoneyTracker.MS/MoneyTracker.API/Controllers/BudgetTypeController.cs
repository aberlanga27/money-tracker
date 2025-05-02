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
/// Controller for the BudgetTypes.
/// </summary>
[ApiController]
[Route($"{Constants.BasePath}/[controller]")]
// [Authorize]
// [ApiExplorerSettings(IgnoreApi = true)]
public class BudgetTypeController(
    ILogger<BudgetTypeController> logger,
    ILocalizationProvider translator,
    IBudgetTypeService budgetTypeService,
    IValidator<BudgetTypeDTO> validator
) : ControllerBase
{
    private readonly ILogger<BudgetTypeController> logger = Guard.Against.Null(logger);
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly IBudgetTypeService budgetTypeService = Guard.Against.Null(budgetTypeService);
    private readonly IValidator<BudgetTypeDTO> validator = Guard.Against.Null(validator);

    /// <summary>
    /// Gets all the BudgetTypes.
    /// </summary>
    /// <param name="pageSize">The maximum number of BudgetTypes to return.</param>
    /// <param name="offsetSize">The number of BudgetTypes to skip.</param>
    /// <returns>A list of BudgetTypes.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<IEnumerable<BudgetTypeDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int? pageSize, int? offsetSize)
    {
        logger.LogDebug("Getting all budgetTypes with pageSize {PageSize} and offsetSize {OffsetSize}", pageSize, offsetSize);

        var budgetTypes = await budgetTypeService.GetAllBudgetTypes(pageSize, offsetSize);
        return Ok(budgetTypes);
    }

    /// <summary>
    /// Gets an BudgetType by its ID.
    /// </summary>
    /// <param name="budgetTypeId">The ID of the BudgetType.</param>
    /// <returns>An BudgetType.</returns>
    [HttpGet("{budgetTypeId}")]
    [ProducesResponseType(typeof(ValueResponse<BudgetTypeDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int budgetTypeId)
    {
        logger.LogDebug("Getting budgetType by id {BudgetTypeId}", budgetTypeId);

        if (budgetTypeId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var budgetType = await budgetTypeService.GetBudgetTypeById(budgetTypeId);
        return Ok(budgetType);
    }

    /// <summary>
    /// Creates a new BudgetType.
    /// </summary>
    /// <param name="budgetTypeDTO">The BudgetType to create.</param>
    /// <returns>A response with the ID of the created BudgetType.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ValueResponse<BudgetTypeDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(BudgetTypeDTO budgetTypeDTO)
    {
        logger.LogDebug("Creating budgetType {BudgetTypeDTO}", JsonConvert.SerializeObject(budgetTypeDTO));

        var validation = await validator.ValidateAsync(budgetTypeDTO, options => options.IncludeRuleSets(Constants.InsertRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await budgetTypeService.CreateBudgetType(budgetTypeDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Updates an BudgetType.
    /// </summary>
    /// <param name="budgetTypeDTO">The BudgetType to update.</param>
    /// <returns>A response with the ID of the updated BudgetType.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(ValueResponse<BudgetTypeDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(BudgetTypeDTO budgetTypeDTO)
    {
        logger.LogDebug("Updating budgetType {BudgetTypeDTO}", JsonConvert.SerializeObject(budgetTypeDTO));

        var validation = await validator.ValidateAsync(budgetTypeDTO, options => options.IncludeRuleSets(Constants.UpdateRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await budgetTypeService.UpdateBudgetType(budgetTypeDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Deletes an BudgetType.
    /// </summary>
    /// <param name="budgetTypeId">The ID of the BudgetType to delete.</param>
    /// <returns>A response with the ID of the deleted BudgetType.</returns>
    [HttpDelete("{budgetTypeId}")]
    [ProducesResponseType(typeof(ValueResponse<BudgetTypeDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int budgetTypeId)
    {
        logger.LogDebug("Deleting budgetType by id {BudgetTypeId}", budgetTypeId);

        if (budgetTypeId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var transactionResponse = await budgetTypeService.DeleteBudgetType(budgetTypeId);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Searches for BudgetTypes by a string.
    /// </summary>
    /// <param name="search">The string to search for.</param>
    /// <returns>A list of BudgetTypes.</returns>
    [HttpGet("Search")]
    [ProducesResponseType(typeof(IEnumerable<BudgetTypeDTO>), StatusCodes.Status200OK)]
    public IActionResult Search(string search)
    {
        logger.LogDebug("Searching budgetTypes by {Search}", search);

        if (string.IsNullOrWhiteSpace(search))
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid search, it can't be null or empty") });

        var budgetTypes = budgetTypeService.SearchBudgetTypes(search);
        return Ok(budgetTypes);
    }

    /// <summary>
    /// Gets an BudgetType by its attributes.
    /// </summary>
    /// <param name="budgetTypeDTO">The attributes of the BudgetType.</param>
    /// <returns>An BudgetType.</returns>
    [HttpPost("Find")]
    [ProducesResponseType(typeof(IEnumerable<BudgetTypeDTO>), StatusCodes.Status200OK)]
    public IActionResult GetByAttributes(BudgetTypeDTO budgetTypeDTO)
    {
        logger.LogDebug("Getting budgetType by attributes {BudgetTypeDTO}", JsonConvert.SerializeObject(budgetTypeDTO));

        var budgetTypes = budgetTypeService.GetBudgetTypesByAttributes(budgetTypeDTO);
        return Ok(budgetTypes);
    }
}