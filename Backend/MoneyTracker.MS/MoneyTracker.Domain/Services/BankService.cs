namespace MoneyTracker.Domain.Services;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Mappers;

public class BankService(
    MoneyTrackerSettings appSettings,
    ILocalizationProvider translator,
    ICacheProvider cache,
    IBankRepository bankRepository
) : PaginatedService(appSettings), IBankService
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly ICacheProvider cache = Guard.Against.Null(cache);
    private readonly IBankRepository bankRepository = Guard.Against.Null(bankRepository);
    private readonly BankMapper mapper = new();

    public IEnumerable<BankDTO> GetAllBanks()
    {
        var banks = bankRepository.GetAllBanks();
        return mapper.Map(banks);
    }

    public async Task<PaginationResponse<IEnumerable<BankDTO>>> GetAllBanks(int? pageSize, int? offsetSize)
    {
        var totalRecords = await bankRepository.GetCount();
        var (size, offset) = ValidatePagination(pageSize, offsetSize, totalRecords);

        var records = bankRepository.GetAllBanks(size, offset);

        return new PaginationResponse<IEnumerable<BankDTO>>
        {
            Status = true,
            Message = translator.T("Entities found", ["Banks"]),
            TotalRecords = totalRecords,
            Response = mapper.Map(records)
        };
    }

    public async Task<ValueResponse<BankDTO>> GetBankById(int bankId)
    {
        var cacheRecord = await cache.GetAsync<BankDTO>($"bank:{bankId}");
        if (cacheRecord != null)
            return new ValueResponse<BankDTO>
            {
                Status = true,
                Message = translator.T("Entity found in cache", ["Bank"]),
                Response = cacheRecord
            };

        var bank = await bankRepository.GetBankById(bankId);
        if (bank == null)
            return new ValueResponse<BankDTO>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Bank"])
            };

        var bankDTO = mapper.Map(bank);
        await cache.SetAsync($"bank:{bankDTO.BankId}", bankDTO);

        return new ValueResponse<BankDTO>
        {
            Status = true,
            Message = translator.T("Entity found", ["Bank"]),
            Response = bankDTO
        };
    }

    public async Task<ValueResponse<BankDTO>> CreateBank(BankDTO bankDTO)
    {
        var bank = mapper.Map(bankDTO);
        var response = await bankRepository.CreateBank(bank);

        if (!response.Status)
            return new ValueResponse<BankDTO> { Status = false, Message = response.Message };

        var bankCreatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"bank:{bankCreatedDTO.BankId}", bankCreatedDTO);

        return new ValueResponse<BankDTO> { Status = true, Message = response.Message, Response = bankCreatedDTO };
    }

    public async Task<ValueResponse<BankDTO>> UpdateBank(BankDTO bankDTO)
    {
        var bank = mapper.Map(bankDTO);
        var response = await bankRepository.UpdateBank(bank);

        if (!response.Status)
            return new ValueResponse<BankDTO> { Status = false, Message = response.Message };

        var bankUpdatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"bank:{bankUpdatedDTO.BankId}", bankUpdatedDTO);

        return new ValueResponse<BankDTO> { Status = true, Message = response.Message, Response = bankUpdatedDTO };
    }

    public async Task<ValueResponse<BankDTO>> DeleteBank(int bankId)
    {
        var response = await bankRepository.DeleteBank(bankId);
        if (!response.Status)
            return new ValueResponse<BankDTO> { Status = false, Message = response.Message };

        var bankDeletedDTO = mapper.Map(response.Response!);
        await cache.RemoveAsync($"bank:{bankDeletedDTO.BankId}");

        return new ValueResponse<BankDTO> { Status = true, Message = response.Message, Response = bankDeletedDTO };
    }

    public IEnumerable<BankDTO> SearchBanks(string search)
    {
        var banks = bankRepository.SearchBanks(search);
        return mapper.Map(banks);
    }

    public IEnumerable<BankDTO> GetBanksByAttributes(BankDTO bankDTO)
    {
        var bank = mapper.Map(bankDTO);
        var banks = bankRepository.GetBanksByAttributes(bank);
        return mapper.Map(banks);
    }
}