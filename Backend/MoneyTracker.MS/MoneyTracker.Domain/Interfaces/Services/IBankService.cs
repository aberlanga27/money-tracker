namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.DTOs;

public interface IBankService
{
    /// <summary>
    /// Get all banks
    /// </summary>
    /// <returns></returns>
    IEnumerable<BankDTO> GetAllBanks();

    /// <summary>
    /// Get all banks with pagination
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="offsetSize"></param>
    /// <returns></returns>
    Task<PaginationResponse<IEnumerable<BankDTO>>> GetAllBanks(int? pageSize, int? offsetSize);

    /// <summary>
    /// Get an bank by its ID
    /// </summary>
    /// <param name="bankId"></param>
    /// <returns></returns>
    Task<ValueResponse<BankDTO>> GetBankById(int bankId);

    /// <summary>
    /// Create an bank
    /// </summary>
    /// <param name="bankDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<BankDTO>> CreateBank(BankDTO bankDTO);

    /// <summary>
    /// Update an bank
    /// </summary>
    /// <param name="bankDTO"></param>
    /// <returns></returns>
    Task<ValueResponse<BankDTO>> UpdateBank(BankDTO bankDTO);

    /// <summary>
    /// Delete an bank
    /// </summary>
    /// <param name="bankId"></param>
    /// <returns></returns>
    Task<ValueResponse<BankDTO>> DeleteBank(int bankId);

    /// <summary>
    /// Search banks by a string search
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<BankDTO> SearchBanks(string search);

    /// <summary>
    /// Get banks by attributes
    /// </summary>
    /// <param name="bankDTO"></param>
    /// <returns></returns>
    IEnumerable<BankDTO> GetBanksByAttributes(BankDTO bankDTO);
}