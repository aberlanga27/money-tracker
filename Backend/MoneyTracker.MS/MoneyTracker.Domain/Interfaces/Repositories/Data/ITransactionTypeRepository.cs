namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;

public interface ITransactionTypeRepository
{
    /// <summary>
    /// Get the count of transactionTypes
    /// </summary>
    /// <returns></returns>
    Task<int> GetCount();

    /// <summary>
    /// Get all transactionTypes
    /// </summary>
    /// <returns></returns>
    IEnumerable<TransactionType> GetAllTransactionTypes();

    /// <summary>
    /// Get all transactionTypes with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    IEnumerable<TransactionType> GetAllTransactionTypes(int pageSize, int offsetSize);

    /// <summary>
    /// Get an transactionType by its ID
    /// </summary>
    /// <param name="transactionTypeId"></param>
    /// <returns></returns>
    Task<TransactionType?> GetTransactionTypeById(int transactionTypeId);

    /// <summary>
    /// Create an transactionType
    /// </summary>
    /// <param name="transactionType"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionType>> CreateTransactionType(TransactionType transactionType);

    /// <summary>
    /// Update an transactionType
    /// </summary>
    /// <param name="transactionType"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionType>> UpdateTransactionType(TransactionType transactionType);

    /// <summary>
    /// Delete an transactionType
    /// </summary>
    /// <param name="transactionTypeId"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionType>> DeleteTransactionType(int transactionTypeId);

    /// <summary>
    /// Search transactionTypes by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<TransactionType> SearchTransactionTypes(string search);

    /// <summary>
    /// Get transactionTypes by attributes
    /// </summary>
    /// <param name="transactionType"></param>
    /// <returns></returns>
    IEnumerable<TransactionType> GetTransactionTypesByAttributes(TransactionType transactionType);
}