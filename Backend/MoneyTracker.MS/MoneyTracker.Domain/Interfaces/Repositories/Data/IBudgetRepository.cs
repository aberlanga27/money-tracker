namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;

public interface IBudgetRepository
{
    /// <summary>
    /// Get the count of budgets
    /// </summary>
    /// <returns></returns>
    Task<int> GetCount();

    /// <summary>
    /// Get all budgets
    /// </summary>
    /// <returns></returns>
    IEnumerable<Budget> GetAllBudgets();

    /// <summary>
    /// Get all budgets with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    IEnumerable<Budget> GetAllBudgets(int pageSize, int offsetSize);

    /// <summary>
    /// Get an budget by its ID
    /// </summary>
    /// <param name="budgetId"></param>
    /// <returns></returns>
    Task<Budget?> GetBudgetById(int budgetId);

    /// <summary>
    /// Create an budget
    /// </summary>
    /// <param name="budget"></param>
    /// <returns></returns>
    Task<ValueResponse<Budget>> CreateBudget(Budget budget);

    /// <summary>
    /// Update an budget
    /// </summary>
    /// <param name="budget"></param>
    /// <returns></returns>
    Task<ValueResponse<Budget>> UpdateBudget(Budget budget);

    /// <summary>
    /// Delete an budget
    /// </summary>
    /// <param name="budgetId"></param>
    /// <returns></returns>
    Task<ValueResponse<Budget>> DeleteBudget(int budgetId);

    /// <summary>
    /// Search budgets by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<Budget> SearchBudgets(string search);

    /// <summary>
    /// Get budgets by attributes
    /// </summary>
    /// <param name="budget"></param>
    /// <returns></returns>
    IEnumerable<Budget> GetBudgetsByAttributes(Budget budget);
}