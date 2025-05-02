namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;

public interface ITransactionTypeService
{
    /// <summary>
    /// Get all transactionTypes
    /// </summary>
    /// <returns></returns>
    IEnumerable<TransactionTypeDTO> GetAllTransactionTypes();

    /// <summary>
    /// Get all transactionTypes with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    Task<PaginationResponse<IEnumerable<TransactionTypeDTO>>> GetAllTransactionTypes(int? pageSize, int? offsetSize);

    /// <summary>
    /// Get an transactionType by its ID
    /// </summary>
    /// <param name="transactionTypeId"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionTypeDTO>> GetTransactionTypeById(int transactionTypeId);

    /// <summary>
    /// Create an transactionType
    /// </summary>
    /// <param name="transactionTypeDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionTypeDTO>> CreateTransactionType(TransactionTypeDTO transactionTypeDTO);

    /// <summary>
    /// Update an transactionType
    /// </summary>
    /// <param name="transactionTypeDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionTypeDTO>> UpdateTransactionType(TransactionTypeDTO transactionTypeDTO);

    /// <summary>
    /// Delete an transactionType
    /// </summary>
    /// <param name="transactionTypeId"></param>
    /// <returns></returns>
    Task<ValueResponse<TransactionTypeDTO>> DeleteTransactionType(int transactionTypeId);

    /// <summary>
    /// Search transactionTypes by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<TransactionTypeDTO> SearchTransactionTypes(string search);

    /// <summary>
    /// Get transactionTypes by attributes
    /// </summary>
    /// <param name="transactionTypeDTO"></param>
    /// <returns></returns>
    IEnumerable<TransactionTypeDTO> GetTransactionTypesByAttributes(TransactionTypeDTO transactionTypeDTO);
}