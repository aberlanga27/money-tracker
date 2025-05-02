namespace MoneyTracker.Infrastructure.Repositories;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Extensions;
using MoneyTracker.Domain.Interfaces;

public class BudgetRepository(
    MoneyTrackerContext context,
    ILocalizationProvider translator
) : Repository<Budget>(context), IBudgetRepository
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);

    private ValueResponse<Budget> ValidateUK(Budget budget)
    {
        // CTX: repository-unique-key, do not remove this line
        return new ValueResponse<Budget> { Status = true, Response = budget };
    }

    private ValueResponse<Budget> ValidateParentFK(Budget budget)
    {
        var budgetType = context.BudgetTypes
            .AsNoTracking()
            .FirstOrDefault(x => x.BudgetTypeId == budget.BudgetTypeId);

        if (budgetType == null)
            return new ValueResponse<Budget> { Status = false, Message = translator.T("Entity not found", ["BudgetType"]) };

        var transactionCategory = context.TransactionCategorys
            .AsNoTracking()
            .FirstOrDefault(x => x.TransactionCategoryId == budget.TransactionCategoryId);

        if (transactionCategory == null)
            return new ValueResponse<Budget> { Status = false, Message = translator.T("Entity not found", ["TransactionCategory"]) };

        // CTX: repository-parent-fk, do not remove this line
        return new ValueResponse<Budget> { Status = true, Response = budget };
    }

    private ValueResponse<Budget> ValidateChildFK(int budgetId)
    {
        // CTX: repository-child-fk, do not remove this line
        return new ValueResponse<Budget> { Status = true, Response = new Budget { BudgetId = budgetId } };
    }

    public virtual async Task<int> GetCount()
    {
        return await context.Budgets.AsNoTracking().CountAsync();
    }

    public virtual IEnumerable<Budget> GetAllBudgets()
    {
        return context.Budgets.AsNoTracking();
    }

    public virtual IEnumerable<Budget> GetAllBudgets(int pageSize, int offsetSize)
    {
        return context.Budgets.AsNoTracking()
            .Include(x => x.TransactionCategory)
            .Include(x => x.BudgetType)
            .OrderBy(x => x.BudgetId)
            .Skip(offsetSize).Take(pageSize);
    }

    public virtual async Task<Budget?> GetBudgetById(int budgetId)
    {
        return await context.Budgets.FirstOrDefaultAsync(x => x.BudgetId == budgetId);
    }

    public virtual async Task<ValueResponse<Budget>> CreateBudget(Budget budget)
    {
        var ukValidation = ValidateUK(budget);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(budget);
        if (!fkValidation.Status)
            return fkValidation;

        var budgetToUpdate = await GetBudgetById(budget.BudgetId);

        if (budgetToUpdate != null)
            return await UpdateBudget(budget);

        budget.BudgetId = 0;
        budget.Created = DateTime.Now;

        await AddAsync(budget);
        return new ValueResponse<Budget>
        {
            Status = true,
            Message = translator.T("Entity created", ["Budget"]),
            Response = budget
        };
    }

    public virtual async Task<ValueResponse<Budget>> UpdateBudget(Budget budget)
    {
        var ukValidation = ValidateUK(budget);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(budget);
        if (!fkValidation.Status)
            return fkValidation;

        var budgetToUpdate = await GetBudgetById(budget.BudgetId);

        if (budgetToUpdate == null)
            return new ValueResponse<Budget>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Budget"])
            };

        budget.Created = budgetToUpdate.Created;
        budget.Modified = DateTime.Now;
        budgetToUpdate.Bind(budget);

        await UpdateAsync(budgetToUpdate);
        return new ValueResponse<Budget>
        {
            Status = true,
            Message = translator.T("Entity updated", ["Budget"]),
            Response = budget
        };
    }

    public virtual async Task<ValueResponse<Budget>> DeleteBudget(int budgetId)
    {
        var fkValidation = ValidateChildFK(budgetId);
        if (!fkValidation.Status)
            return fkValidation;

        var budget = await GetBudgetById(budgetId);

        if (budget == null)
            return new ValueResponse<Budget>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Budget"])
            };

        await DeleteAsync(budget);
        return new ValueResponse<Budget>
        {
            Status = true,
            Message = translator.T("Entity deleted", ["Budget"]),
            Response = budget
        };
    }

    public virtual IEnumerable<Budget> SearchBudgets(string search)
    {
        return context.Budgets
            .AsNoTracking()
            .Include(x => x.TransactionCategory)
            .Include(x => x.BudgetType)
            .Where(x =>
                // CTX: repository-search, do not remove this line
                false
            );
    }

    public virtual IEnumerable<Budget> GetBudgetsByAttributes(Budget budget)
    {
        var queryableBudgets = context.Budgets.AsNoTracking()
            .Include(x => x.TransactionCategory)
            .Include(x => x.BudgetType)
            .AsQueryable();

        var isByAttributesFilter = false;

        if (budget.BudgetId > 0)
        {
            queryableBudgets = queryableBudgets.Where(x => x.BudgetId == budget.BudgetId);
            isByAttributesFilter = true;
        }

        if (budget.TransactionCategoryId > 0)
        {
            queryableBudgets = queryableBudgets.Where(x => x.TransactionCategoryId == budget.TransactionCategoryId);
            isByAttributesFilter = true;
        }

        if (budget.BudgetTypeId > 0)
        {
            queryableBudgets = queryableBudgets.Where(x => x.BudgetTypeId == budget.BudgetTypeId);
            isByAttributesFilter = true;
        }

        if (budget.BudgetAmount > 0)
        {
            queryableBudgets = queryableBudgets.Where(x => x.BudgetAmount == budget.BudgetAmount);
            isByAttributesFilter = true;
        }

        // CTX: repository-attribute, do not remove this line

        return isByAttributesFilter ? queryableBudgets : [];
    }
}