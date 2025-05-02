namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;

public interface ITransactionRepository
{
    /// <summary>
    /// Get the count of transactions
    /// </summary>
    /// <returns></returns>
    Task<int> GetCount();

    /// <summary>
    /// Get all transactions
    /// </summary>
    /// <returns></returns>
    IEnumerable<Transaction> GetAllTransactions();

    /// <summary>
    /// Get all transactions with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    IEnumerable<Transaction> GetAllTransactions(int pageSize, int offsetSize);

    /// <summary>
    /// Get an transaction by its ID
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    Task<Transaction?> GetTransactionById(int transactionId);

    /// <summary>
    /// Create an transaction
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    Task<ValueResponse<Transaction>> CreateTransaction(Transaction transaction);

    /// <summary>
    /// Update an transaction
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    Task<ValueResponse<Transaction>> UpdateTransaction(Transaction transaction);

    /// <summary>
    /// Delete an transaction
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    Task<ValueResponse<Transaction>> DeleteTransaction(int transactionId);

    /// <summary>
    /// Search transactions by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<Transaction> SearchTransactions(string search);

    /// <summary>
    /// Get transactions by attributes
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    IEnumerable<Transaction> GetTransactionsByAttributes(Transaction transaction);

    // ...

    /// <summary>
    /// Get transactions grouped by category
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    IEnumerable<TransactionsGroupedByCategory> GetTransactionsGroupedByCategory(DateTime startDate, DateTime endDate);
}