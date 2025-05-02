namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;

public interface ITransactionCategoryRepository
{
    /// <summary>
    /// Get the count of transactionCategorys
    /// </summary>
    /// <returns></returns>
    Task<int> GetCount();

    /// <summary>
    /// Get all transactionCategorys
    /// </summary>
    /// <returns></returns>
    IEnumerable<TransactionCategory> GetAllTransactionCategorys();

    /// <summary>
    /// Get all transactionCategorys with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    IEnumerable<TransactionCategory> GetAllTransactionCategorys(int pageSize, int offsetSize);

    /// <summary>
    /// Get an transactionCategory by its ID
    /// </summary>
    /// <param name="transactionCategoryId"></param>
    /// <returns></returns>
    Task<TransactionCategory?> GetTransactionCategoryById(int transactionCategoryId);

    /// <summary>
    /// Create an transactionCategory
    /// </summary>
    /// <param name="transactionCategory"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionCategory>> CreateTransactionCategory(TransactionCategory transactionCategory);

    /// <summary>
    /// Update an transactionCategory
    /// </summary>
    /// <param name="transactionCategory"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionCategory>> UpdateTransactionCategory(TransactionCategory transactionCategory);

    /// <summary>
    /// Delete an transactionCategory
    /// </summary>
    /// <param name="transactionCategoryId"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionCategory>> DeleteTransactionCategory(int transactionCategoryId);

    /// <summary>
    /// Search transactionCategorys by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<TransactionCategory> SearchTransactionCategorys(string search);

    /// <summary>
    /// Get transactionCategorys by attributes
    /// </summary>
    /// <param name="transactionCategory"></param>
    /// <returns></returns>
    IEnumerable<TransactionCategory> GetTransactionCategorysByAttributes(TransactionCategory transactionCategory);
}