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
/// Controller for the Banks.
/// </summary>
[ApiController]
[Route($"{Constants.BasePath}/[controller]")]
// [Authorize]
// [ApiExplorerSettings(IgnoreApi = true)]
public class BankController(
    ILogger<BankController> logger,
    ILocalizationProvider translator,
    IBankService bankService,
    IValidator<BankDTO> validator
) : ControllerBase
{
    private readonly ILogger<BankController> logger = Guard.Against.Null(logger);
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly IBankService bankService = Guard.Against.Null(bankService);
    private readonly IValidator<BankDTO> validator = Guard.Against.Null(validator);

    /// <summary>
    /// Gets all the Banks.
    /// </summary>
    /// <param name="pageSize">The maximum number of Banks to return.</param>
    /// <param name="offsetSize">The number of Banks to skip.</param>
    /// <returns>A list of Banks.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<IEnumerable<BankDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int? pageSize, int? offsetSize)
    {
        logger.LogDebug("Getting all banks with pageSize {PageSize} and offsetSize {OffsetSize}", pageSize, offsetSize);

        var banks = await bankService.GetAllBanks(pageSize, offsetSize);
        return Ok(banks);
    }

    /// <summary>
    /// Gets an Bank by its ID.
    /// </summary>
    /// <param name="bankId">The ID of the Bank.</param>
    /// <returns>An Bank.</returns>
    [HttpGet("{bankId}")]
    [ProducesResponseType(typeof(ValueResponse<BankDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int bankId)
    {
        logger.LogDebug("Getting bank by id {BankId}", bankId);

        if (bankId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var bank = await bankService.GetBankById(bankId);
        return Ok(bank);
    }

    /// <summary>
    /// Creates a new Bank.
    /// </summary>
    /// <param name="bankDTO">The Bank to create.</param>
    /// <returns>A response with the ID of the created Bank.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ValueResponse<BankDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(BankDTO bankDTO)
    {
        logger.LogDebug("Creating bank {BankDTO}", JsonConvert.SerializeObject(bankDTO));

        var validation = await validator.ValidateAsync(bankDTO, options => options.IncludeRuleSets(Constants.InsertRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await bankService.CreateBank(bankDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Updates an Bank.
    /// </summary>
    /// <param name="bankDTO">The Bank to update.</param>
    /// <returns>A response with the ID of the updated Bank.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(ValueResponse<BankDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValueResponse<List<ValidationFailure>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(BankDTO bankDTO)
    {
        logger.LogDebug("Updating bank {BankDTO}", JsonConvert.SerializeObject(bankDTO));

        var validation = await validator.ValidateAsync(bankDTO, options => options.IncludeRuleSets(Constants.UpdateRuleSet));
        if (!validation.IsValid)
            return BadRequest(new ValueResponse<List<ValidationFailure>>
            {
                Status = false,
                Message = translator.T("Invalid data, verify the fields"),
                Response = validation.Errors
            });

        var transactionResponse = await bankService.UpdateBank(bankDTO);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Deletes an Bank.
    /// </summary>
    /// <param name="bankId">The ID of the Bank to delete.</param>
    /// <returns>A response with the ID of the deleted Bank.</returns>
    [HttpDelete("{bankId}")]
    [ProducesResponseType(typeof(ValueResponse<BankDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int bankId)
    {
        logger.LogDebug("Deleting bank by id {BankId}", bankId);

        if (bankId <= 0)
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid Id, it can't be null or less than 0") });

        var transactionResponse = await bankService.DeleteBank(bankId);
        return Ok(transactionResponse);
    }

    /// <summary>
    /// Searches for Banks by a string.
    /// </summary>
    /// <param name="search">The string to search for.</param>
    /// <returns>A list of Banks.</returns>
    [HttpGet("Search")]
    [ProducesResponseType(typeof(IEnumerable<BankDTO>), StatusCodes.Status200OK)]
    public IActionResult Search(string search)
    {
        logger.LogDebug("Searching banks by {Search}", search);

        if (string.IsNullOrWhiteSpace(search))
            return BadRequest(new TransactionResponse { Status = false, Message = translator.T("Invalid search, it can't be null or empty") });

        var banks = bankService.SearchBanks(search);
        return Ok(banks);
    }

    /// <summary>
    /// Gets an Bank by its attributes.
    /// </summary>
    /// <param name="bankDTO">The attributes of the Bank.</param>
    /// <returns>An Bank.</returns>
    [HttpPost("Find")]
    [ProducesResponseType(typeof(IEnumerable<BankDTO>), StatusCodes.Status200OK)]
    public IActionResult GetByAttributes(BankDTO bankDTO)
    {
        logger.LogDebug("Getting bank by attributes {BankDTO}", JsonConvert.SerializeObject(bankDTO));

        var banks = bankService.GetBanksByAttributes(bankDTO);
        return Ok(banks);
    }
}