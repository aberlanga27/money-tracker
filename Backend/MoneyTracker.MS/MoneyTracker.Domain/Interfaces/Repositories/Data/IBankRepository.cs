namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;

public interface IBankRepository
{
    /// <summary>
    /// Get the count of banks
    /// </summary>
    /// <returns></returns>
    Task<int> GetCount();

    /// <summary>
    /// Get all banks
    /// </summary>
    /// <returns></returns>
    IEnumerable<Bank> GetAllBanks();

    /// <summary>
    /// Get all banks with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    IEnumerable<Bank> GetAllBanks(int pageSize, int offsetSize);

    /// <summary>
    /// Get an bank by its ID
    /// </summary>
    /// <param name="bankId"></param>
    /// <returns></returns>
    Task<Bank?> GetBankById(int bankId);

    /// <summary>
    /// Create an bank
    /// </summary>
    /// <param name="bank"></param>
    /// <returns></returns>
    Task<ValueResponse<Bank>> CreateBank(Bank bank);

    /// <summary>
    /// Update an bank
    /// </summary>
    /// <param name="bank"></param>
    /// <returns></returns>
    Task<ValueResponse<Bank>> UpdateBank(Bank bank);

    /// <summary>
    /// Delete an bank
    /// </summary>
    /// <param name="bankId"></param>
    /// <returns></returns>
    Task<ValueResponse<Bank>> DeleteBank(int bankId);

    /// <summary>
    /// Search banks by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<Bank> SearchBanks(string search);

    /// <summary>
    /// Get banks by attributes
    /// </summary>
    /// <param name="bank"></param>
    /// <returns></returns>
    IEnumerable<Bank> GetBanksByAttributes(Bank bank);
}