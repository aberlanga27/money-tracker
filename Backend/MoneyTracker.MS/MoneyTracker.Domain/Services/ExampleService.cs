namespace MoneyTracker.Domain.Services;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Mappers;

public class ExampleService(
    MoneyTrackerSettings appSettings,
    ILocalizationProvider translator,
    ICacheProvider cache,
    IExampleRepository exampleRepository
) : PaginatedService(appSettings), IExampleService
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);
    private readonly ICacheProvider cache = Guard.Against.Null(cache);
    private readonly IExampleRepository exampleRepository = Guard.Against.Null(exampleRepository);
    private readonly ExampleMapper mapper = new();

    public IEnumerable<ExampleDTO> GetAllExamples()
    {
        var examples = exampleRepository.GetAllExamples();
        return mapper.Map(examples);
    }

    public async Task<PaginationResponse<IEnumerable<ExampleDTO>>> GetAllExamples(int? pageSize, int? offsetSize)
    {
        var totalRecords = await exampleRepository.GetCount();
        var (size, offset) = ValidatePagination(pageSize, offsetSize, totalRecords);

        var records = exampleRepository.GetAllExamples(size, offset);

        return new PaginationResponse<IEnumerable<ExampleDTO>>
        {
            Status = true,
            Message = translator.T("Entities found", ["Examples"]),
            TotalRecords = totalRecords,
            Response = mapper.Map(records)
        };
    }

    public async Task<ValueResponse<ExampleDTO>> GetExampleById(int exampleId)
    {
        var cacheRecord = await cache.GetAsync<ExampleDTO>($"example:{exampleId}");
        if (cacheRecord != null)
            return new ValueResponse<ExampleDTO>
            {
                Status = true,
                Message = translator.T("Entity found in cache", ["Example"]),
                Response = cacheRecord
            };

        var example = await exampleRepository.GetExampleById(exampleId);
        if (example == null)
            return new ValueResponse<ExampleDTO>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Example"])
            };

        var exampleDTO = mapper.Map(example);
        await cache.SetAsync($"example:{exampleDTO.ExampleId}", exampleDTO);

        return new ValueResponse<ExampleDTO>
        {
            Status = true,
            Message = translator.T("Entity found", ["Example"]),
            Response = exampleDTO
        };
    }

    public async Task<ValueResponse<ExampleDTO>> CreateExample(ExampleDTO exampleDTO)
    {
        var example = mapper.Map(exampleDTO);
        var response = await exampleRepository.CreateExample(example);

        if (!response.Status)
            return new ValueResponse<ExampleDTO> { Status = false, Message = response.Message };

        var exampleCreatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"example:{exampleCreatedDTO.ExampleId}", exampleCreatedDTO);

        return new ValueResponse<ExampleDTO> { Status = true, Message = response.Message, Response = exampleCreatedDTO };
    }

    public async Task<ValueResponse<ExampleDTO>> UpdateExample(ExampleDTO exampleDTO)
    {
        var example = mapper.Map(exampleDTO);
        var response = await exampleRepository.UpdateExample(example);

        if (!response.Status)
            return new ValueResponse<ExampleDTO> { Status = false, Message = response.Message };

        var exampleUpdatedDTO = mapper.Map(response.Response!);
        await cache.SetAsync($"example:{exampleUpdatedDTO.ExampleId}", exampleUpdatedDTO);

        return new ValueResponse<ExampleDTO> { Status = true, Message = response.Message, Response = exampleUpdatedDTO };
    }

    public async Task<ValueResponse<ExampleDTO>> DeleteExample(int exampleId)
    {
        var response = await exampleRepository.DeleteExample(exampleId);
        if (!response.Status)
            return new ValueResponse<ExampleDTO> { Status = false, Message = response.Message };

        var exampleDeletedDTO = mapper.Map(response.Response!);
        await cache.RemoveAsync($"example:{exampleDeletedDTO.ExampleId}");

        return new ValueResponse<ExampleDTO> { Status = true, Message = response.Message, Response = exampleDeletedDTO };
    }

    public IEnumerable<ExampleDTO> SearchExamples(string search)
    {
        var examples = exampleRepository.SearchExamples(search);
        return mapper.Map(examples);
    }

    public IEnumerable<ExampleDTO> GetExamplesByAttributes(ExampleDTO exampleDTO)
    {
        var example = mapper.Map(exampleDTO);
        var examples = exampleRepository.GetExamplesByAttributes(example);
        return mapper.Map(examples);
    }
}