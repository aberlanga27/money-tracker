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
/// Controller for the Budgets.
/// </summary>
[ApiController]
[Route($"{Constants.BasePath}/[controller]")]
// [Authorize]
// [ApiExplorerSettings(IgnoreApi = true)]
public class BudgetController(
    ILogger<BudgetController> logger,
    ILocalizationProvider translator,
    IBudgetService budgetService,
    IValidator<BudgetDTO> validator
) : ControllerBase
{
    private readonly ILogger<BudgetController> logger = Guard.Against.Null(logger);
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly IBudgetService budgetService = Guard.Against.Null(budgetService);
    private readonly IValidator<BudgetDTO> validator = Guard.Against.Null(validator);

    /// <summary>
    /// Gets all the Budgets.
    /// </summary>
    /// <param name="pageSize">The maximum number of Budgets to return.</param>
    /// <param name="offsetSize">The number of Budgets to skip.</param>
    /// <returns>A list of Budgets.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<IEnumerable<BudgetAttributesDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int? pageSize, int? offsetSize)
    {
        logger.LogDebug("Getting all budgets with pageSize {PageSize} and offsetSize {OffsetSize}", pageSize, offsetSize);

        var budgets = await budgetService.GetAllBudgets(pageSize, offsetSize);
        return Ok(budgets);
    }

    /// <summary>
    /// Gets an Budget by its ID.
    /// </summary>
    /// <param name="budgetId">The ID of the Budget.</param>
    /// <returns>An Budget.</returns>
    [HttpGet("{budgetId}")]
    [ProducesResponseType(typeof(ValueResponse<BudgetDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int budgetId)
    {
        logger.LogDebug("Getting budget by id {BudgetId}", budgetId);

        if (budgetId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var budget = await budgetService.GetBudgetById(budgetId);
        return Ok(budget);
    }

    /// <summary>
    /// Creates a new Budget.
    /// </summary>
    /// <param name="budgetDTO">The Budget to create.</param>
    /// <returns>A response with the ID of the created Budget.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ValueResponse<BudgetDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(BudgetDTO budgetDTO)
    {
        logger.LogDebug("Creating budget {BudgetDTO}", JsonConvert.SerializeObject(budgetDTO));

        var validation = await validator.ValidateAsync(budgetDTO, options => options.IncludeRuleSets(Constants.InsertRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await budgetService.CreateBudget(budgetDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Updates an Budget.
    /// </summary>
    /// <param name="budgetDTO">The Budget to update.</param>
    /// <returns>A response with the ID of the updated Budget.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(ValueResponse<BudgetDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(BudgetDTO budgetDTO)
    {
        logger.LogDebug("Updating budget {BudgetDTO}", JsonConvert.SerializeObject(budgetDTO));

        var validation = await validator.ValidateAsync(budgetDTO, options => options.IncludeRuleSets(Constants.UpdateRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await budgetService.UpdateBudget(budgetDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Deletes an Budget.
    /// </summary>
    /// <param name="budgetId">The ID of the Budget to delete.</param>
    /// <returns>A response with the ID of the deleted Budget.</returns>
    [HttpDelete("{budgetId}")]
    [ProducesResponseType(typeof(ValueResponse<BudgetDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int budgetId)
    {
        logger.LogDebug("Deleting budget by id {BudgetId}", budgetId);

        if (budgetId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var transactionResponse = await budgetService.DeleteBudget(budgetId);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Searches for Budgets by a string.
    /// </summary>
    /// <param name="search">The string to search for.</param>
    /// <returns>A list of Budgets.</returns>
    [HttpGet("Search")]
    [ProducesResponseType(typeof(IEnumerable<BudgetAttributesDTO>), StatusCodes.Status200OK)]
    public IActionResult Search(string search)
    {
        logger.LogDebug("Searching budgets by {Search}", search);

        if (string.IsNullOrWhiteSpace(search))
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid search, it can't be null or empty") });

        var budgets = budgetService.SearchBudgets(search);
        return Ok(budgets);
    }

    /// <summary>
    /// Gets an Budget by its attributes.
    /// </summary>
    /// <param name="budgetDTO">The attributes of the Budget.</param>
    /// <returns>An Budget.</returns>
    [HttpPost("Find")]
    [ProducesResponseType(typeof(IEnumerable<BudgetAttributesDTO>), StatusCodes.Status200OK)]
    public IActionResult GetByAttributes(BudgetDTO budgetDTO)
    {
        logger.LogDebug("Getting budget by attributes {BudgetDTO}", JsonConvert.SerializeObject(budgetDTO));

        var budgets = budgetService.GetBudgetsByAttributes(budgetDTO);
        return Ok(budgets);
    }
}