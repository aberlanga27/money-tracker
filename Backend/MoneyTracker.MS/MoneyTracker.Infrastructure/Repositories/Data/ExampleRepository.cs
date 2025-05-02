namespace MoneyTracker.Infrastructure.Repositories;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Extensions;
using MoneyTracker.Domain.Interfaces;

public class ExampleRepository(
    MoneyTrackerContext context,
    ILocalizationProvider translator
) : Repository<Example>(context), IExampleRepository
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);

    private ValueResponse<Example> ValidateUK(Example example)
    {
        // CTX: repository-unique-key, do not remove this line
        return new ValueResponse<Example> { Status = true, Response = example };
    }

    private ValueResponse<Example> ValidateParentFK(Example example)
    {
        // CTX: repository-parent-fk, do not remove this line
        return new ValueResponse<Example> { Status = true, Response = example };
    }

    private ValueResponse<Example> ValidateChildFK(int exampleId)
    {
        // CTX: repository-child-fk, do not remove this line
        return new ValueResponse<Example> { Status = true, Response = new Example { ExampleId = exampleId } };
    }

    public virtual async Task<int> GetCount()
    {
        return await context.Examples.AsNoTracking().CountAsync();
    }

    public virtual IEnumerable<Example> GetAllExamples()
    {
        return context.Examples.AsNoTracking();
    }

    public virtual IEnumerable<Example> GetAllExamples(int pageSize, int offsetSize)
    {
        return context.Examples.AsNoTracking()
            .OrderBy(x => x.ExampleId)
            .Skip(offsetSize).Take(pageSize);
    }

    public virtual async Task<Example?> GetExampleById(int exampleId)
    {
        return await context.Examples.FirstOrDefaultAsync(x => x.ExampleId == exampleId);
    }

    public virtual async Task<ValueResponse<Example>> CreateExample(Example example)
    {
        var ukValidation = ValidateUK(example);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(example);
        if (!fkValidation.Status)
            return fkValidation;

        var exampleToUpdate = await GetExampleById(example.ExampleId);

        if (exampleToUpdate != null)
            return await UpdateExample(example);

        example.ExampleId = 0;
        example.Created = DateTime.Now;

        await AddAsync(example);
        return new ValueResponse<Example>
        {
            Status = true,
            Message = translator.T("Entity created", ["Example"]),
            Response = example
        };
    }

    public virtual async Task<ValueResponse<Example>> UpdateExample(Example example)
    {
        var ukValidation = ValidateUK(example);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(example);
        if (!fkValidation.Status)
            return fkValidation;

        var exampleToUpdate = await GetExampleById(example.ExampleId);

        if (exampleToUpdate == null)
            return new ValueResponse<Example>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Example"])
            };

        example.Created = exampleToUpdate.Created;
        example.Modified = DateTime.Now;
        exampleToUpdate.Bind(example);

        await UpdateAsync(exampleToUpdate);
        return new ValueResponse<Example>
        {
            Status = true,
            Message = translator.T("Entity updated", ["Example"]),
            Response = example
        };
    }

    public virtual async Task<ValueResponse<Example>> DeleteExample(int exampleId)
    {
        var fkValidation = ValidateChildFK(exampleId);
        if (!fkValidation.Status)
            return fkValidation;

        var example = await GetExampleById(exampleId);

        if (example == null)
            return new ValueResponse<Example>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Example"])
            };

        await DeleteAsync(example);
        return new ValueResponse<Example>
        {
            Status = true,
            Message = translator.T("Entity deleted", ["Example"]),
            Response = example
        };
    }

    public virtual IEnumerable<Example> SearchExamples(string search)
    {
        return context.Examples
            .AsNoTracking()
            .Where(x =>
                // CTX: repository-search, do not remove this line
                false
            );
    }

    public virtual IEnumerable<Example> GetExamplesByAttributes(Example example)
    {
        var queryableExamples = context.Examples.AsNoTracking()
            .AsQueryable();

        var isByAttributesFilter = false;

        if (example.ExampleId > 0)
        {
            queryableExamples = queryableExamples.Where(x => x.ExampleId == example.ExampleId);
            isByAttributesFilter = true;
        }

        // CTX: repository-attribute, do not remove this line

        return isByAttributesFilter ? queryableExamples : [];
    }
}