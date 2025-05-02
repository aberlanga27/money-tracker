namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;

public interface IBudgetTypeService
{
    /// <summary>
    /// Get all budgetTypes
    /// </summary>
    /// <returns></returns>
    IEnumerable<BudgetTypeDTO> GetAllBudgetTypes();

    /// <summary>
    /// Get all budgetTypes with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    Task<PaginationResponse<IEnumerable<BudgetTypeDTO>>> GetAllBudgetTypes(int? pageSize, int? offsetSize);

    /// <summary>
    /// Get an budgetType by its ID
    /// </summary>
    /// <param name="budgetTypeId"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetTypeDTO>> GetBudgetTypeById(int budgetTypeId);

    /// <summary>
    /// Create an budgetType
    /// </summary>
    /// <param name="budgetTypeDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetTypeDTO>> CreateBudgetType(BudgetTypeDTO budgetTypeDTO);

    /// <summary>
    /// Update an budgetType
    /// </summary>
    /// <param name="budgetTypeDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetTypeDTO>> UpdateBudgetType(BudgetTypeDTO budgetTypeDTO);

    /// <summary>
    /// Delete an budgetType
    /// </summary>
    /// <param name="budgetTypeId"></param>
    /// <returns></returns>
    Task<ValueResponse<BudgetTypeDTO>> DeleteBudgetType(int budgetTypeId);

    /// <summary>
    /// Search budgetTypes by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<BudgetTypeDTO> SearchBudgetTypes(string search);

    /// <summary>
    /// Get budgetTypes by attributes
    /// </summary>
    /// <param name="budgetTypeDTO"></param>
    /// <returns></returns>
    IEnumerable<BudgetTypeDTO> GetBudgetTypesByAttributes(BudgetTypeDTO budgetTypeDTO);
}