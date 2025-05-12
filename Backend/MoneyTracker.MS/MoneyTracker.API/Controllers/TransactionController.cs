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
/// Controller for the Transactions.
/// </summary>
[ApiController]
[Route($"{Constants.BasePath}/[controller]")]
// [Authorize]
// [ApiExplorerSettings(IgnoreApi = true)]
public class TransactionController(
    ILogger<TransactionController> logger,
    ILocalizationProvider translator,
    ITransactionService transactionService,
    IValidator<TransactionDTO> validator
) : ControllerBase
{
    private readonly ILogger<TransactionController> logger = Guard.Against.Null(logger);
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly ITransactionService transactionService = Guard.Against.Null(transactionService);
    private readonly IValidator<TransactionDTO> validator = Guard.Against.Null(validator);

    /// <summary>
    /// Gets all the Transactions.
    /// </summary>
    /// <param name="pageSize">The maximum number of Transactions to return.</param>
    /// <param name="offsetSize">The number of Transactions to skip.</param>
    /// <returns>A list of Transactions.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<IEnumerable<TransactionAttributesDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int? pageSize, int? offsetSize)
    {
        logger.LogDebug("Getting all transactions with pageSize {PageSize} and offsetSize {OffsetSize}", pageSize, offsetSize);

        var transactions = await transactionService.GetAllTransactions(pageSize, offsetSize);
        return Ok(transactions);
    }

    /// <summary>
    /// Gets an Transaction by its ID.
    /// </summary>
    /// <param name="transactionId">The ID of the Transaction.</param>
    /// <returns>An Transaction.</returns>
    [HttpGet("{transactionId}")]
    [ProducesResponseType(typeof(ValueResponse<TransactionDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int transactionId)
    {
        logger.LogDebug("Getting transaction by id {TransactionId}", transactionId);

        if (transactionId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var transaction = await transactionService.GetTransactionById(transactionId);
        return Ok(transaction);
    }

    /// <summary>
    /// Creates a new Transaction.
    /// </summary>
    /// <param name="transactionDTO">The Transaction to create.</param>
    /// <returns>A response with the ID of the created Transaction.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ValueResponse<TransactionDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(TransactionDTO transactionDTO)
    {
        logger.LogDebug("Creating transaction {TransactionDTO}", JsonConvert.SerializeObject(transactionDTO));

        var validation = await validator.ValidateAsync(transactionDTO, options => options.IncludeRuleSets(Constants.InsertRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await transactionService.CreateTransaction(transactionDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Updates an Transaction.
    /// </summary>
    /// <param name="transactionDTO">The Transaction to update.</param>
    /// <returns>A response with the ID of the updated Transaction.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(ValueResponse<TransactionDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(TransactionDTO transactionDTO)
    {
        logger.LogDebug("Updating transaction {TransactionDTO}", JsonConvert.SerializeObject(transactionDTO));

        var validation = await validator.ValidateAsync(transactionDTO, options => options.IncludeRuleSets(Constants.UpdateRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await transactionService.UpdateTransaction(transactionDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Deletes an Transaction.
    /// </summary>
    /// <param name="transactionId">The ID of the Transaction to delete.</param>
    /// <returns>A response with the ID of the deleted Transaction.</returns>
    [HttpDelete("{transactionId}")]
    [ProducesResponseType(typeof(ValueResponse<TransactionDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int transactionId)
    {
        logger.LogDebug("Deleting transaction by id {TransactionId}", transactionId);

        if (transactionId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var transactionResponse = await transactionService.DeleteTransaction(transactionId);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Searches for Transactions by a string.
    /// </summary>
    /// <param name="search">The string to search for.</param>
    /// <returns>A list of Transactions.</returns>
    [HttpGet("Search")]
    [ProducesResponseType(typeof(IEnumerable<TransactionAttributesDTO>), StatusCodes.Status200OK)]
    public IActionResult Search(string search)
    {
        logger.LogDebug("Searching transactions by {Search}", search);

        if (string.IsNullOrWhiteSpace(search))
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid search, it can't be null or empty") });

        var transactions = transactionService.SearchTransactions(search);
        return Ok(transactions);
    }

    /// <summary>
    /// Gets an Transaction by its attributes.
    /// </summary>
    /// <param name="transactionDTO">The attributes of the Transaction.</param>
    /// <returns>An Transaction.</returns>
    [HttpPost("Find")]
    [ProducesResponseType(typeof(IEnumerable<TransactionAttributesDTO>), StatusCodes.Status200OK)]
    public IActionResult GetByAttributes(TransactionDTO transactionDTO)
    {
        logger.LogDebug("Getting transaction by attributes {TransactionDTO}", JsonConvert.SerializeObject(transactionDTO));

        var transactions = transactionService.GetTransactionsByAttributes(transactionDTO);
        return Ok(transactions);
    }

    // ...

    /// <summary>
    /// Gets transactions grouped by category.
    /// </summary>
    /// <param name="dateRange">The date range to filter transactions.</param>
    /// <returns>A list of transactions grouped by category.</returns>
    [HttpPost("GroupByCategory")]
    [ProducesResponseType(typeof(ValueResponse<IEnumerable<TransactionsGroupedByCategoryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public IActionResult GetTransactionsGroupedByCategory(DateTimeRangeDTO dateRange)
    {
        logger.LogDebug("Getting transactions grouped by category from {StartDate} to {EndDate}", dateRange.StartDate, dateRange.EndDate);

        if (dateRange.StartDate == default || dateRange.EndDate == default || dateRange.StartDate > dateRange.EndDate)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid date range") });

        var transactions = transactionService.GetTransactionsGroupedByCategory(dateRange.StartDate, dateRange.EndDate);
        return Ok(transactions);
    }

    /// <summary>
    /// Gets transactions grouped by bank.
    /// </summary>
    /// <param name="dateRange">The date range to filter transactions.</param>
    /// <returns>A list of transactions grouped by bank.</returns>
    [HttpPost("GroupByBank")]
    [ProducesResponseType(typeof(ValueResponse<IEnumerable<TransactionsGroupedByBankDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public IActionResult GetTransactionsGroupedByBank(DateTimeRangeDTO dateRange)
    {
        logger.LogDebug("Getting transactions grouped by bank from {StartDate} to {EndDate}", dateRange.StartDate, dateRange.EndDate);

        if (dateRange.StartDate == default || dateRange.EndDate == default || dateRange.StartDate > dateRange.EndDate)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid date range") });

        var transactions = transactionService.GetTransactionsGroupedByBank(dateRange.StartDate, dateRange.EndDate);
        return Ok(transactions);
    }
}