namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;

public interface ITransactionService
{
    /// <summary>
    /// Get all transactions
    /// </summary>
    /// <returns></returns>
    IEnumerable<TransactionDTO> GetAllTransactions();

    /// <summary>
    /// Get all transactions with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    Task<PaginationResponse<IEnumerable<TransactionAttributesDTO>>> GetAllTransactions(int? pageSize, int? offsetSize);

    /// <summary>
    /// Get an transaction by its ID
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionDTO>> GetTransactionById(int transactionId);

    /// <summary>
    /// Create an transaction
    /// </summary>
    /// <param name="transactionDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionDTO>> CreateTransaction(TransactionDTO transactionDTO);

    /// <summary>
    /// Update an transaction
    /// </summary>
    /// <param name="transactionDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionDTO>> UpdateTransaction(TransactionDTO transactionDTO);

    /// <summary>
    /// Delete an transaction
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionDTO>> DeleteTransaction(int transactionId);

    /// <summary>
    /// Search transactions by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<TransactionAttributesDTO> SearchTransactions(string search);

    /// <summary>
    /// Get transactions by attributes
    /// </summary>
    /// <param name="transactionDTO"></param>
    /// <returns></returns>
    IEnumerable<TransactionAttributesDTO> GetTransactionsByAttributes(TransactionDTO transactionDTO);

    // ...

    /// <summary>
    /// Get transactions grouped by category
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    ValueResponse<IEnumerable<TransactionsGroupedByCategoryDTO>> GetTransactionsGroupedByCategory(DateTime startDate, DateTime endDate);
}