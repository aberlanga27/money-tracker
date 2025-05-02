namespace MoneyTracker.Domain.Services;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Mappers;

public class TransactionTypeService(
    MoneyTrackerSettings appSettings,
    ILocalizationProvider translator,
    ICacheProvider cache,
    ITransactionTypeRepository transactionTypeRepository
) : PaginatedService(appSettings), ITransactionTypeService
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly ICacheProvider cache = Guard.Against.Null(cache);
    private readonly ITransactionTypeRepository transactionTypeRepository = Guard.Against.Null(transactionTypeRepository);
    private readonly TransactionTypeMapper mapper = new();

    public IEnumerable<TransactionTypeDTO> GetAllTransactionTypes()
    {
        var transactionTypes = transactionTypeRepository.GetAllTransactionTypes();
        return mapper.Map(transactionTypes);
    }

    public async Task<PaginationResponse<IEnumerable<TransactionTypeDTO>>> GetAllTransactionTypes(int? pageSize, int? offsetSize)
    {
        var totalRecords = await transactionTypeRepository.GetCount();
        var (size, offset) = ValidatePagination(pageSize, offsetSize, totalRecords);

        var records = transactionTypeRepository.GetAllTransactionTypes(size, offset);

        return new PaginationResponse<IEnumerable<TransactionTypeDTO>>
        {
            Status = true,
            Message = translator.T("Entities found", ["TransactionTypes"]),
            TotalRecords = totalRecords,
            Response = mapper.Map(records)
        };
    }

    public async Task<ValueResponse<TransactionTypeDTO>> GetTransactionTypeById(int transactionTypeId)
    {
        var cacheRecord = await cache.GetAsync<TransactionTypeDTO>($"transactionType:{transactionTypeId}");
        if (cacheRecord != null)
            return new ValueResponse<TransactionTypeDTO>
            {
                Status = true,
                Message = translator.T("Entity found in cache", ["TransactionType"]),
                Response = cacheRecord
            };

        var transactionType = await transactionTypeRepository.GetTransactionTypeById(transactionTypeId);
        if (transactionType == null)
            return new ValueResponse<TransactionTypeDTO>
            {
                Status = false,
                Message = translator.T("Entity not found", ["TransactionType"])
            };

        var transactionTypeDTO = mapper.Map(transactionType);
        await cache.SetAsync($"transactionType:{transactionTypeDTO.TransactionTypeId}", transactionTypeDTO);

        return new ValueResponse<TransactionTypeDTO>
        {
            Status = true,
            Message = translator.T("Entity found", ["TransactionType"]),
            Response = transactionTypeDTO
        };
    }

    public async Task<ValueResponse<TransactionTypeDTO>> CreateTransactionType(TransactionTypeDTO transactionTypeDTO)
    {
        var transactionType = mapper.Map(transactionTypeDTO);
        var response = await transactionTypeRepository.CreateTransactionType(transactionType);

        if (!response.Status)
            return new ValueResponse<TransactionTypeDTO> { Status = false, Message = response.Message };

        var transactionTypeCreatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"transactionType:{transactionTypeCreatedDTO.TransactionTypeId}", transactionTypeCreatedDTO);

        return new ValueResponse<TransactionTypeDTO> { Status = true, Message = response.Message, Response = transactionTypeCreatedDTO };
    }

    public async Task<ValueResponse<TransactionTypeDTO>> UpdateTransactionType(TransactionTypeDTO transactionTypeDTO)
    {
        var transactionType = mapper.Map(transactionTypeDTO);
        var response = await transactionTypeRepository.UpdateTransactionType(transactionType);

        if (!response.Status)
            return new ValueResponse<TransactionTypeDTO> { Status = false, Message = response.Message };

        var transactionTypeUpdatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"transactionType:{transactionTypeUpdatedDTO.TransactionTypeId}", transactionTypeUpdatedDTO);

        return new ValueResponse<TransactionTypeDTO> { Status = true, Message = response.Message, Response = transactionTypeUpdatedDTO };
    }

    public async Task<ValueResponse<TransactionTypeDTO>> DeleteTransactionType(int transactionTypeId)
    {
        var response = await transactionTypeRepository.DeleteTransactionType(transactionTypeId);
        if (!response.Status)
            return new ValueResponse<TransactionTypeDTO> { Status = false, Message = response.Message };

        var transactionTypeDeletedDTO = mapper.Map(response.Response!);
        await cache.RemoveAsync($"transactionType:{transactionTypeDeletedDTO.TransactionTypeId}");

        return new ValueResponse<TransactionTypeDTO> { Status = true, Message = response.Message, Response = transactionTypeDeletedDTO };
    }

    public IEnumerable<TransactionTypeDTO> SearchTransactionTypes(string search)
    {
        var transactionTypes = transactionTypeRepository.SearchTransactionTypes(search);
        return mapper.Map(transactionTypes);
    }

    public IEnumerable<TransactionTypeDTO> GetTransactionTypesByAttributes(TransactionTypeDTO transactionTypeDTO)
    {
        var transactionType = mapper.Map(transactionTypeDTO);
        var transactionTypes = transactionTypeRepository.GetTransactionTypesByAttributes(transactionType);
        return mapper.Map(transactionTypes);
    }
}