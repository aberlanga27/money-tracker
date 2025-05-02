namespace MoneyTracker.Infrastructure.Repositories;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Extensions;
using MoneyTracker.Domain.Interfaces;

public class BudgetTypeRepository(
    MoneyTrackerContext context,
    ILocalizationProvider translator
) : Repository<BudgetType>(context), IBudgetTypeRepository
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);

    private ValueResponse<BudgetType> ValidateUK(BudgetType budgetType)
    {
        var ukBudgetTypeName = context.BudgetTypes
            .AsNoTracking()
            .FirstOrDefault(x => x.BudgetTypeId != budgetType.BudgetTypeId && x.BudgetTypeName == budgetType.BudgetTypeName);

        if (ukBudgetTypeName != null)
            return new ValueResponse<BudgetType> { Status = false, Message = translator.T("Field cannot be duplicated", ["BudgetTypeName"]) };

        // CTX: repository-unique-key, do not remove this line
        return new ValueResponse<BudgetType> { Status = true, Response = budgetType };
    }

    private ValueResponse<BudgetType> ValidateParentFK(BudgetType budgetType)
    {
        // CTX: repository-parent-fk, do not remove this line
        return new ValueResponse<BudgetType> { Status = true, Response = budgetType };
    }

    private ValueResponse<BudgetType> ValidateChildFK(int budgetTypeId)
    {
        var budgets = context.Budgets
            .AsNoTracking()
            .FirstOrDefault(x => x.BudgetTypeId == budgetTypeId);

        if (budgets != null)
            return new ValueResponse<BudgetType> { Status = false, Message = translator.T("Entity has child dependencies", ["Budget"]) };

        // CTX: repository-child-fk, do not remove this line
        return new ValueResponse<BudgetType> { Status = true, Response = new BudgetType { BudgetTypeId = budgetTypeId } };
    }

    public virtual async Task<int> GetCount()
    {
        return await context.BudgetTypes.AsNoTracking().CountAsync();
    }

    public virtual IEnumerable<BudgetType> GetAllBudgetTypes()
    {
        return context.BudgetTypes.AsNoTracking();
    }

    public virtual IEnumerable<BudgetType> GetAllBudgetTypes(int pageSize, int offsetSize)
    {
        return context.BudgetTypes.AsNoTracking()
            .OrderBy(x => x.BudgetTypeId)
            .Skip(offsetSize).Take(pageSize);
    }

    public virtual async Task<BudgetType?> GetBudgetTypeById(int budgetTypeId)
    {
        return await context.BudgetTypes.FirstOrDefaultAsync(x => x.BudgetTypeId == budgetTypeId);
    }

    public virtual async Task<ValueResponse<BudgetType>> CreateBudgetType(BudgetType budgetType)
    {
        var ukValidation = ValidateUK(budgetType);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(budgetType);
        if (!fkValidation.Status)
            return fkValidation;

        var budgetTypeToUpdate = await GetBudgetTypeById(budgetType.BudgetTypeId);

        if (budgetTypeToUpdate != null)
            return await UpdateBudgetType(budgetType);

        budgetType.BudgetTypeId = 0;
        budgetType.Created = DateTime.Now;

        await AddAsync(budgetType);
        return new ValueResponse<BudgetType>
        {
            Status = true,
            Message = translator.T("Entity created", ["BudgetType"]),
            Response = budgetType
        };
    }

    public virtual async Task<ValueResponse<BudgetType>> UpdateBudgetType(BudgetType budgetType)
    {
        var ukValidation = ValidateUK(budgetType);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(budgetType);
        if (!fkValidation.Status)
            return fkValidation;

        var budgetTypeToUpdate = await GetBudgetTypeById(budgetType.BudgetTypeId);

        if (budgetTypeToUpdate == null)
            return new ValueResponse<BudgetType>
            {
                Status = false,
                Message = translator.T("Entity not found", ["BudgetType"])
            };

        budgetType.Created = budgetTypeToUpdate.Created;
        budgetType.Modified = DateTime.Now;
        budgetTypeToUpdate.Bind(budgetType);

        await UpdateAsync(budgetTypeToUpdate);
        return new ValueResponse<BudgetType>
        {
            Status = true,
            Message = translator.T("Entity updated", ["BudgetType"]),
            Response = budgetType
        };
    }

    public virtual async Task<ValueResponse<BudgetType>> DeleteBudgetType(int budgetTypeId)
    {
        var fkValidation = ValidateChildFK(budgetTypeId);
        if (!fkValidation.Status)
            return fkValidation;

        var budgetType = await GetBudgetTypeById(budgetTypeId);

        if (budgetType == null)
            return new ValueResponse<BudgetType>
            {
                Status = false,
                Message = translator.T("Entity not found", ["BudgetType"])
            };

        await DeleteAsync(budgetType);
        return new ValueResponse<BudgetType>
        {
            Status = true,
            Message = translator.T("Entity deleted", ["BudgetType"]),
            Response = budgetType
        };
    }

    public virtual IEnumerable<BudgetType> SearchBudgetTypes(string search)
    {
        return context.BudgetTypes
            .AsNoTracking()
            .Where(x =>
                x.BudgetTypeName.Contains(search) ||
                // CTX: repository-search, do not remove this line
                false
            );
    }

    public virtual IEnumerable<BudgetType> GetBudgetTypesByAttributes(BudgetType budgetType)
    {
        var queryableBudgetTypes = context.BudgetTypes.AsNoTracking()
            .AsQueryable();

        var isByAttributesFilter = false;

        if (budgetType.BudgetTypeId > 0)
        {
            queryableBudgetTypes = queryableBudgetTypes.Where(x => x.BudgetTypeId == budgetType.BudgetTypeId);
            isByAttributesFilter = true;
        }

        if (!string.IsNullOrEmpty(budgetType.BudgetTypeName))
        {
            queryableBudgetTypes = queryableBudgetTypes.Where(x => x.BudgetTypeName == budgetType.BudgetTypeName);
            isByAttributesFilter = true;
        }

        if (budgetType.BudgetTypeDays > 0)
        {
            queryableBudgetTypes = queryableBudgetTypes.Where(x => x.BudgetTypeDays == budgetType.BudgetTypeDays);
            isByAttributesFilter = true;
        }

        // CTX: repository-attribute, do not remove this line

        return isByAttributesFilter ? queryableBudgetTypes : [];
    }
}