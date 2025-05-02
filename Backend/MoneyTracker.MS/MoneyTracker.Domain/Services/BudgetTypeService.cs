namespace MoneyTracker.Domain.Services;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Mappers;

public class BudgetTypeService(
    MoneyTrackerSettings appSettings,
    ILocalizationProvider translator,
    ICacheProvider cache,
    IBudgetTypeRepository budgetTypeRepository
) : PaginatedService(appSettings), IBudgetTypeService
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly ICacheProvider cache = Guard.Against.Null(cache);
    private readonly IBudgetTypeRepository budgetTypeRepository = Guard.Against.Null(budgetTypeRepository);
    private readonly BudgetTypeMapper mapper = new();

    public IEnumerable<BudgetTypeDTO> GetAllBudgetTypes()
    {
        var budgetTypes = budgetTypeRepository.GetAllBudgetTypes();
        return mapper.Map(budgetTypes);
    }

    public async Task<PaginationResponse<IEnumerable<BudgetTypeDTO>>> GetAllBudgetTypes(int? pageSize, int? offsetSize)
    {
        var totalRecords = await budgetTypeRepository.GetCount();
        var (size, offset) = ValidatePagination(pageSize, offsetSize, totalRecords);

        var records = budgetTypeRepository.GetAllBudgetTypes(size, offset);

        return new PaginationResponse<IEnumerable<BudgetTypeDTO>>
        {
            Status = true,
            Message = translator.T("Entities found", ["BudgetTypes"]),
            TotalRecords = totalRecords,
            Response = mapper.Map(records)
        };
    }

    public async Task<ValueResponse<BudgetTypeDTO>> GetBudgetTypeById(int budgetTypeId)
    {
        var cacheRecord = await cache.GetAsync<BudgetTypeDTO>($"budgetType:{budgetTypeId}");
        if (cacheRecord != null)
            return new ValueResponse<BudgetTypeDTO>
            {
                Status = true,
                Message = translator.T("Entity found in cache", ["BudgetType"]),
                Response = cacheRecord
            };

        var budgetType = await budgetTypeRepository.GetBudgetTypeById(budgetTypeId);
        if (budgetType == null)
            return new ValueResponse<BudgetTypeDTO>
            {
                Status = false,
                Message = translator.T("Entity not found", ["BudgetType"])
            };

        var budgetTypeDTO = mapper.Map(budgetType);
        await cache.SetAsync($"budgetType:{budgetTypeDTO.BudgetTypeId}", budgetTypeDTO);

        return new ValueResponse<BudgetTypeDTO>
        {
            Status = true,
            Message = translator.T("Entity found", ["BudgetType"]),
            Response = budgetTypeDTO
        };
    }

    public async Task<ValueResponse<BudgetTypeDTO>> CreateBudgetType(BudgetTypeDTO budgetTypeDTO)
    {
        var budgetType = mapper.Map(budgetTypeDTO);
        var response = await budgetTypeRepository.CreateBudgetType(budgetType);

        if (!response.Status)
            return new ValueResponse<BudgetTypeDTO> { Status = false, Message = response.Message };

        var budgetTypeCreatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"budgetType:{budgetTypeCreatedDTO.BudgetTypeId}", budgetTypeCreatedDTO);

        return new ValueResponse<BudgetTypeDTO> { Status = true, Message = response.Message, Response = budgetTypeCreatedDTO };
    }

    public async Task<ValueResponse<BudgetTypeDTO>> UpdateBudgetType(BudgetTypeDTO budgetTypeDTO)
    {
        var budgetType = mapper.Map(budgetTypeDTO);
        var response = await budgetTypeRepository.UpdateBudgetType(budgetType);

        if (!response.Status)
            return new ValueResponse<BudgetTypeDTO> { Status = false, Message = response.Message };

        var budgetTypeUpdatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"budgetType:{budgetTypeUpdatedDTO.BudgetTypeId}", budgetTypeUpdatedDTO);

        return new ValueResponse<BudgetTypeDTO> { Status = true, Message = response.Message, Response = budgetTypeUpdatedDTO };
    }

    public async Task<ValueResponse<BudgetTypeDTO>> DeleteBudgetType(int budgetTypeId)
    {
        var response = await budgetTypeRepository.DeleteBudgetType(budgetTypeId);
        if (!response.Status)
            return new ValueResponse<BudgetTypeDTO> { Status = false, Message = response.Message };

        var budgetTypeDeletedDTO = mapper.Map(response.Response!);
        await cache.RemoveAsync($"budgetType:{budgetTypeDeletedDTO.BudgetTypeId}");

        return new ValueResponse<BudgetTypeDTO> { Status = true, Message = response.Message, Response = budgetTypeDeletedDTO };
    }

    public IEnumerable<BudgetTypeDTO> SearchBudgetTypes(string search)
    {
        var budgetTypes = budgetTypeRepository.SearchBudgetTypes(search);
        return mapper.Map(budgetTypes);
    }

    public IEnumerable<BudgetTypeDTO> GetBudgetTypesByAttributes(BudgetTypeDTO budgetTypeDTO)
    {
        var budgetType = mapper.Map(budgetTypeDTO);
        var budgetTypes = budgetTypeRepository.GetBudgetTypesByAttributes(budgetType);
        return mapper.Map(budgetTypes);
    }
}