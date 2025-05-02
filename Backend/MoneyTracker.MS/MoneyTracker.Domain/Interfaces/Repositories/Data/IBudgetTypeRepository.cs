namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;

public interface IBudgetTypeRepository
{
    /// <summary>
    /// Get the count of budgetTypes
    /// </summary>
    /// <returns></returns>
    Task<int> GetCount();

    /// <summary>
    /// Get all budgetTypes
    /// </summary>
    /// <returns></returns>
    IEnumerable<BudgetType> GetAllBudgetTypes();

    /// <summary>
    /// Get all budgetTypes with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    IEnumerable<BudgetType> GetAllBudgetTypes(int pageSize, int offsetSize);

    /// <summary>
    /// Get an budgetType by its ID
    /// </summary>
    /// <param name="budgetTypeId"></param>
    /// <returns></returns>
    Task<BudgetType?> GetBudgetTypeById(int budgetTypeId);

    /// <summary>
    /// Create an budgetType
    /// </summary>
    /// <param name="budgetType"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetType>> CreateBudgetType(BudgetType budgetType);

    /// <summary>
    /// Update an budgetType
    /// </summary>
    /// <param name="budgetType"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetType>> UpdateBudgetType(BudgetType budgetType);

    /// <summary>
    /// Delete an budgetType
    /// </summary>
    /// <param name="budgetTypeId"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetType>> DeleteBudgetType(int budgetTypeId);

    /// <summary>
    /// Search budgetTypes by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<BudgetType> SearchBudgetTypes(string search);

    /// <summary>
    /// Get budgetTypes by attributes
    /// </summary>
    /// <param name="budgetType"></param>
    /// <returns></returns>
    IEnumerable<BudgetType> GetBudgetTypesByAttributes(BudgetType budgetType);
}