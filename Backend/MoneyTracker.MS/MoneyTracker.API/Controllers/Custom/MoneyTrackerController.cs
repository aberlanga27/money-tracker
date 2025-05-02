namespace MoneyTracker.API.Controllers;

using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using MoneyTracker.API.Common;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Interfaces;

/// <summary>
/// Controller for MoneyTracker
/// </summary>
[ApiController]
[Route($"{Constants.BasePath}/[controller]")]
// [Authorize]
// [ApiExplorerSettings(IgnoreApi = true)]
// [ExcludeFromCodeCoverage]
public class MoneyTrackerController(
    IMoneyTrackerService moneyTrackerService
) : ControllerBase
{
    private readonly IMoneyTrackerService moneyTrackerService = Guard.Against.Null(moneyTrackerService);

    /// <summary>
    /// Check the status of the API
    /// </summary>
    /// <returns>Return true if the API has connection to the DB</returns>
    [HttpGet("HealthCheckup")]
    [ProducesResponseType(typeof(ValueResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> HealthCheckup()
    {
        var response = await moneyTrackerService.HealthCheckup();
        return Ok(response);
    }
}