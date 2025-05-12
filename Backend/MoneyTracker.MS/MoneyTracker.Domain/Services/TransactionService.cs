namespace MoneyTracker.Domain.Services;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Mappers;

public class TransactionService(
    MoneyTrackerSettings appSettings,
    ILocalizationProvider translator,
    ICacheProvider cache,
    ITransactionRepository transactionRepository
) : PaginatedService(appSettings), ITransactionService
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly ICacheProvider cache = Guard.Against.Null(cache);
    private readonly ITransactionRepository transactionRepository = Guard.Against.Null(transactionRepository);
    private readonly TransactionMapper mapper = new();

    public IEnumerable<TransactionDTO> GetAllTransactions()
    {
        var transactions = transactionRepository.GetAllTransactions();
        return mapper.Map(transactions);
    }

    public async Task<PaginationResponse<IEnumerable<TransactionAttributesDTO>>> GetAllTransactions(int? pageSize, int? offsetSize)
    {
        var totalRecords = await transactionRepository.GetCount();
        var (size, offset) = ValidatePagination(pageSize, offsetSize, totalRecords);

        var records = transactionRepository.GetAllTransactions(size, offset);

        return new PaginationResponse<IEnumerable<TransactionAttributesDTO>>
        {
            Status = true,
            Message = translator.T("Entities found", ["Transactions"]),
            TotalRecords = totalRecords,
            Response = mapper.MapAttributes(records)
        };
    }

    public async Task<ValueResponse<TransactionDTO>> GetTransactionById(int transactionId)
    {
        var cacheRecord = await cache.GetAsync<TransactionDTO>($"transaction:{transactionId}");
        if (cacheRecord != null)
            return new ValueResponse<TransactionDTO>
            {
                Status = true,
                Message = translator.T("Entity found in cache", ["Transaction"]),
                Response = cacheRecord
            };

        var transaction = await transactionRepository.GetTransactionById(transactionId);
        if (transaction == null)
            return new ValueResponse<TransactionDTO>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Transaction"])
            };

        var transactionDTO = mapper.Map(transaction);
        await cache.SetAsync($"transaction:{transactionDTO.TransactionId}", transactionDTO);

        return new ValueResponse<TransactionDTO>
        {
            Status = true,
            Message = translator.T("Entity found", ["Transaction"]),
            Response = transactionDTO
        };
    }

    public async Task<ValueResponse<TransactionDTO>> CreateTransaction(TransactionDTO transactionDTO)
    {
        var transaction = mapper.Map(transactionDTO);
        var response = await transactionRepository.CreateTransaction(transaction);

        if (!response.Status)
            return new ValueResponse<TransactionDTO> { Status = false, Message = response.Message };

        var transactionCreatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"transaction:{transactionCreatedDTO.TransactionId}", transactionCreatedDTO);

        return new ValueResponse<TransactionDTO> { Status = true, Message = response.Message, Response = transactionCreatedDTO };
    }

    public async Task<ValueResponse<TransactionDTO>> UpdateTransaction(TransactionDTO transactionDTO)
    {
        var transaction = mapper.Map(transactionDTO);
        var response = await transactionRepository.UpdateTransaction(transaction);

        if (!response.Status)
            return new ValueResponse<TransactionDTO> { Status = false, Message = response.Message };

        var transactionUpdatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"transaction:{transactionUpdatedDTO.TransactionId}", transactionUpdatedDTO);

        return new ValueResponse<TransactionDTO> { Status = true, Message = response.Message, Response = transactionUpdatedDTO };
    }

    public async Task<ValueResponse<TransactionDTO>> DeleteTransaction(int transactionId)
    {
        var response = await transactionRepository.DeleteTransaction(transactionId);
        if (!response.Status)
            return new ValueResponse<TransactionDTO> { Status = false, Message = response.Message };

        var transactionDeletedDTO = mapper.Map(response.Response!);
        await cache.RemoveAsync($"transaction:{transactionDeletedDTO.TransactionId}");

        return new ValueResponse<TransactionDTO> { Status = true, Message = response.Message, Response = transactionDeletedDTO };
    }

    public IEnumerable<TransactionAttributesDTO> SearchTransactions(string search)
    {
        var transactions = transactionRepository.SearchTransactions(search);
        return mapper.MapAttributes(transactions);
    }

    public IEnumerable<TransactionAttributesDTO> GetTransactionsByAttributes(TransactionDTO transactionDTO)
    {
        var transaction = mapper.Map(transactionDTO);
        var transactions = transactionRepository.GetTransactionsByAttributes(transaction);
        return mapper.MapAttributes(transactions);
    }

    // ...

    public ValueResponse<IEnumerable<TransactionsGroupedByCategoryDTO>> GetTransactionsGroupedByCategory(DateTime startDate, DateTime endDate)
    {
        var transactions = transactionRepository.GetTransactionsGroupedByCategory(startDate, endDate);
        return new ValueResponse<IEnumerable<TransactionsGroupedByCategoryDTO>>
        {
            Status = true,
            Message = translator.T("Entities found", ["Transactions"]),
            Response = mapper.Map(transactions)
        };
    }

    public ValueResponse<IEnumerable<TransactionsGroupedByBankDTO>> GetTransactionsGroupedByBank(DateTime startDate, DateTime endDate)
    {
        var transactions = transactionRepository.GetTransactionsGroupedByBank(startDate, endDate);
        return new ValueResponse<IEnumerable<TransactionsGroupedByBankDTO>>
        {
            Status = true,
            Message = translator.T("Entities found", ["Transactions"]),
            Response = mapper.Map(transactions)
        };
    }
}