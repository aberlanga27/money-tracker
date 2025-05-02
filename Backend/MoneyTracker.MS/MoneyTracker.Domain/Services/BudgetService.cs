namespace MoneyTracker.Domain.Services;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Mappers;

public class BudgetService(
    MoneyTrackerSettings appSettings,
    ILocalizationProvider translator,
    ICacheProvider cache,
    IBudgetRepository budgetRepository
) : PaginatedService(appSettings), IBudgetService
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly ICacheProvider cache = Guard.Against.Null(cache);
    private readonly IBudgetRepository budgetRepository = Guard.Against.Null(budgetRepository);
    private readonly BudgetMapper mapper = new();

    public IEnumerable<BudgetDTO> GetAllBudgets()
    {
        var budgets = budgetRepository.GetAllBudgets();
        return mapper.Map(budgets);
    }

    public async Task<PaginationResponse<IEnumerable<BudgetAttributesDTO>>> GetAllBudgets(int? pageSize, int? offsetSize)
    {
        var totalRecords = await budgetRepository.GetCount();
        var (size, offset) = ValidatePagination(pageSize, offsetSize, totalRecords);

        var records = budgetRepository.GetAllBudgets(size, offset);

        return new PaginationResponse<IEnumerable<BudgetAttributesDTO>>
        {
            Status = true,
            Message = translator.T("Entities found", ["Budgets"]),
            TotalRecords = totalRecords,
            Response = mapper.MapAttributes(records)
        };
    }

    public async Task<ValueResponse<BudgetDTO>> GetBudgetById(int budgetId)
    {
        var cacheRecord = await cache.GetAsync<BudgetDTO>($"budget:{budgetId}");
        if (cacheRecord != null)
            return new ValueResponse<BudgetDTO>
            {
                Status = true,
                Message = translator.T("Entity found in cache", ["Budget"]),
                Response = cacheRecord
            };

        var budget = await budgetRepository.GetBudgetById(budgetId);
        if (budget == null)
            return new ValueResponse<BudgetDTO>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Budget"])
            };

        var budgetDTO = mapper.Map(budget);
        await cache.SetAsync($"budget:{budgetDTO.BudgetId}", budgetDTO);

        return new ValueResponse<BudgetDTO>
        {
            Status = true,
            Message = translator.T("Entity found", ["Budget"]),
            Response = budgetDTO
        };
    }

    public async Task<ValueResponse<BudgetDTO>> CreateBudget(BudgetDTO budgetDTO)
    {
        var budget = mapper.Map(budgetDTO);
        var response = await budgetRepository.CreateBudget(budget);

        if (!response.Status)
            return new ValueResponse<BudgetDTO> { Status = false, Message = response.Message };

        var budgetCreatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"budget:{budgetCreatedDTO.BudgetId}", budgetCreatedDTO);

        return new ValueResponse<BudgetDTO> { Status = true, Message = response.Message, Response = budgetCreatedDTO };
    }

    public async Task<ValueResponse<BudgetDTO>> UpdateBudget(BudgetDTO budgetDTO)
    {
        var budget = mapper.Map(budgetDTO);
        var response = await budgetRepository.UpdateBudget(budget);

        if (!response.Status)
            return new ValueResponse<BudgetDTO> { Status = false, Message = response.Message };

        var budgetUpdatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"budget:{budgetUpdatedDTO.BudgetId}", budgetUpdatedDTO);

        return new ValueResponse<BudgetDTO> { Status = true, Message = response.Message, Response = budgetUpdatedDTO };
    }

    public async Task<ValueResponse<BudgetDTO>> DeleteBudget(int budgetId)
    {
        var response = await budgetRepository.DeleteBudget(budgetId);
        if (!response.Status)
            return new ValueResponse<BudgetDTO> { Status = false, Message = response.Message };

        var budgetDeletedDTO = mapper.Map(response.Response!);
        await cache.RemoveAsync($"budget:{budgetDeletedDTO.BudgetId}");

        return new ValueResponse<BudgetDTO> { Status = true, Message = response.Message, Response = budgetDeletedDTO };
    }

    public IEnumerable<BudgetAttributesDTO> SearchBudgets(string search)
    {
        var budgets = budgetRepository.SearchBudgets(search);
        return mapper.MapAttributes(budgets);
    }

    public IEnumerable<BudgetAttributesDTO> GetBudgetsByAttributes(BudgetDTO budgetDTO)
    {
        var budget = mapper.Map(budgetDTO);
        var budgets = budgetRepository.GetBudgetsByAttributes(budget);
        return mapper.MapAttributes(budgets);
    }
}