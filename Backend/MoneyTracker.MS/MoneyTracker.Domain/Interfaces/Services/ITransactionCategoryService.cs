namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;

public interface ITransactionCategoryService
{
    /// <summary>
    /// Get all transactionCategorys
    /// </summary>
    /// <returns></returns>
    IEnumerable<TransactionCategoryDTO> GetAllTransactionCategorys();

    /// <summary>
    /// Get all transactionCategorys with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    Task<PaginationResponse<IEnumerable<TransactionCategoryDTO>>> GetAllTransactionCategorys(int? pageSize, int? offsetSize);

    /// <summary>
    /// Get an transactionCategory by its ID
    /// </summary>
    /// <param name="transactionCategoryId"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionCategoryDTO>> GetTransactionCategoryById(int transactionCategoryId);

    /// <summary>
    /// Create an transactionCategory
    /// </summary>
    /// <param name="transactionCategoryDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionCategoryDTO>> CreateTransactionCategory(TransactionCategoryDTO transactionCategoryDTO);

    /// <summary>
    /// Update an transactionCategory
    /// </summary>
    /// <param name="transactionCategoryDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionCategoryDTO>> UpdateTransactionCategory(TransactionCategoryDTO transactionCategoryDTO);

    /// <summary>
    /// Delete an transactionCategory
    /// </summary>
    /// <param name="transactionCategoryId"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionCategoryDTO>> DeleteTransactionCategory(int transactionCategoryId);

    /// <summary>
    /// Search transactionCategorys by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<TransactionCategoryDTO> SearchTransactionCategorys(string search);

    /// <summary>
    /// Get transactionCategorys by attributes
    /// </summary>
    /// <param name="transactionCategoryDTO"></param>
    /// <returns></returns>
    IEnumerable<TransactionCategoryDTO> GetTransactionCategorysByAttributes(TransactionCategoryDTO transactionCategoryDTO);
}