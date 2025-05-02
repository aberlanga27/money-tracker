namespace MoneyTracker.Domain.Services;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Mappers;

public class TransactionCategoryService(
    MoneyTrackerSettings appSettings,
    ILocalizationProvider translator,
    ICacheProvider cache,
    ITransactionCategoryRepository transactionCategoryRepository
) : PaginatedService(appSettings), ITransactionCategoryService
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly ICacheProvider cache = Guard.Against.Null(cache);
    private readonly ITransactionCategoryRepository transactionCategoryRepository = Guard.Against.Null(transactionCategoryRepository);
    private readonly TransactionCategoryMapper mapper = new();

    public IEnumerable<TransactionCategoryDTO> GetAllTransactionCategorys()
    {
        var transactionCategorys = transactionCategoryRepository.GetAllTransactionCategorys();
        return mapper.Map(transactionCategorys);
    }

    public async Task<PaginationResponse<IEnumerable<TransactionCategoryDTO>>> GetAllTransactionCategorys(int? pageSize, int? offsetSize)
    {
        var totalRecords = await transactionCategoryRepository.GetCount();
        var (size, offset) = ValidatePagination(pageSize, offsetSize, totalRecords);

        var records = transactionCategoryRepository.GetAllTransactionCategorys(size, offset);

        return new PaginationResponse<IEnumerable<TransactionCategoryDTO>>
        {
            Status = true,
            Message = translator.T("Entities found", ["TransactionCategorys"]),
            TotalRecords = totalRecords,
            Response = mapper.Map(records)
        };
    }

    public async Task<ValueResponse<TransactionCategoryDTO>> GetTransactionCategoryById(int transactionCategoryId)
    {
        var cacheRecord = await cache.GetAsync<TransactionCategoryDTO>($"transactionCategory:{transactionCategoryId}");
        if (cacheRecord != null)
            return new ValueResponse<TransactionCategoryDTO>
            {
                Status = true,
                Message = translator.T("Entity found in cache", ["TransactionCategory"]),
                Response = cacheRecord
            };

        var transactionCategory = await transactionCategoryRepository.GetTransactionCategoryById(transactionCategoryId);
        if (transactionCategory == null)
            return new ValueResponse<TransactionCategoryDTO>
            {
                Status = false,
                Message = translator.T("Entity not found", ["TransactionCategory"])
            };

        var transactionCategoryDTO = mapper.Map(transactionCategory);
        await cache.SetAsync($"transactionCategory:{transactionCategoryDTO.TransactionCategoryId}", transactionCategoryDTO);

        return new ValueResponse<TransactionCategoryDTO>
        {
            Status = true,
            Message = translator.T("Entity found", ["TransactionCategory"]),
            Response = transactionCategoryDTO
        };
    }

    public async Task<ValueResponse<TransactionCategoryDTO>> CreateTransactionCategory(TransactionCategoryDTO transactionCategoryDTO)
    {
        var transactionCategory = mapper.Map(transactionCategoryDTO);
        var response = await transactionCategoryRepository.CreateTransactionCategory(transactionCategory);

        if (!response.Status)
            return new ValueResponse<TransactionCategoryDTO> { Status = false, Message = response.Message };

        var transactionCategoryCreatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"transactionCategory:{transactionCategoryCreatedDTO.TransactionCategoryId}", transactionCategoryCreatedDTO);

        return new ValueResponse<TransactionCategoryDTO> { Status = true, Message = response.Message, Response = transactionCategoryCreatedDTO };
    }

    public async Task<ValueResponse<TransactionCategoryDTO>> UpdateTransactionCategory(TransactionCategoryDTO transactionCategoryDTO)
    {
        var transactionCategory = mapper.Map(transactionCategoryDTO);
        var response = await transactionCategoryRepository.UpdateTransactionCategory(transactionCategory);

        if (!response.Status)
            return new ValueResponse<TransactionCategoryDTO> { Status = false, Message = response.Message };

        var transactionCategoryUpdatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"transactionCategory:{transactionCategoryUpdatedDTO.TransactionCategoryId}", transactionCategoryUpdatedDTO);

        return new ValueResponse<TransactionCategoryDTO> { Status = true, Message = response.Message, Response = transactionCategoryUpdatedDTO };
    }

    public async Task<ValueResponse<TransactionCategoryDTO>> DeleteTransactionCategory(int transactionCategoryId)
    {
        var response = await transactionCategoryRepository.DeleteTransactionCategory(transactionCategoryId);
        if (!response.Status)
            return new ValueResponse<TransactionCategoryDTO> { Status = false, Message = response.Message };

        var transactionCategoryDeletedDTO = mapper.Map(response.Response!);
        await cache.RemoveAsync($"transactionCategory:{transactionCategoryDeletedDTO.TransactionCategoryId}");

        return new ValueResponse<TransactionCategoryDTO> { Status = true, Message = response.Message, Response = transactionCategoryDeletedDTO };
    }

    public IEnumerable<TransactionCategoryDTO> SearchTransactionCategorys(string search)
    {
        var transactionCategorys = transactionCategoryRepository.SearchTransactionCategorys(search);
        return mapper.Map(transactionCategorys);
    }

    public IEnumerable<TransactionCategoryDTO> GetTransactionCategorysByAttributes(TransactionCategoryDTO transactionCategoryDTO)
    {
        var transactionCategory = mapper.Map(transactionCategoryDTO);
        var transactionCategorys = transactionCategoryRepository.GetTransactionCategorysByAttributes(transactionCategory);
        return mapper.Map(transactionCategorys);
    }
}