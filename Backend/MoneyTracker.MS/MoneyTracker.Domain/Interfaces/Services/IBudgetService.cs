namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;

public interface IBudgetService
{
    /// <summary>
    /// Get all budgets
    /// </summary>
    /// <returns></returns>
    IEnumerable<BudgetDTO> GetAllBudgets();

    /// <summary>
    /// Get all budgets with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    Task<PaginationResponse<IEnumerable<BudgetAttributesDTO>>> GetAllBudgets(int? pageSize, int? offsetSize);

    /// <summary>
    /// Get an budget by its ID
    /// </summary>
    /// <param name="budgetId"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetDTO>> GetBudgetById(int budgetId);

    /// <summary>
    /// Create an budget
    /// </summary>
    /// <param name="budgetDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetDTO>> CreateBudget(BudgetDTO budgetDTO);

    /// <summary>
    /// Update an budget
    /// </summary>
    /// <param name="budgetDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetDTO>> UpdateBudget(BudgetDTO budgetDTO);

    /// <summary>
    /// Delete an budget
    /// </summary>
    /// <param name="budgetId"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetDTO>> DeleteBudget(int budgetId);

    /// <summary>
    /// Search budgets by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<BudgetAttributesDTO> SearchBudgets(string search);

    /// <summary>
    /// Get budgets by attributes
    /// </summary>
    /// <param name="budgetDTO"></param>
    /// <returns></returns>
    IEnumerable<BudgetAttributesDTO> GetBudgetsByAttributes(BudgetDTO budgetDTO);
}