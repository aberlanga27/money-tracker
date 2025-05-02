namespace MoneyTracker.Infrastructure.Repositories;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Extensions;
using MoneyTracker.Domain.Interfaces;

public class TransactionCategoryRepository(
    MoneyTrackerContext context,
    ILocalizationProvider translator
) : Repository<TransactionCategory>(context), ITransactionCategoryRepository
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);

    private ValueResponse<TransactionCategory> ValidateUK(TransactionCategory transactionCategory)
    {
        var ukTransactionCategoryColor = context.TransactionCategorys
            .AsNoTracking()
            .FirstOrDefault(x => x.TransactionCategoryId != transactionCategory.TransactionCategoryId && x.TransactionCategoryColor == transactionCategory.TransactionCategoryColor);

        if (ukTransactionCategoryColor != null)
            return new ValueResponse<TransactionCategory> { Status = false, Message = translator.T("Field cannot be duplicated", ["TransactionCategoryColor"]) };

        var ukTransactionCategoryIcon = context.TransactionCategorys
            .AsNoTracking()
            .FirstOrDefault(x => x.TransactionCategoryId != transactionCategory.TransactionCategoryId && x.TransactionCategoryIcon == transactionCategory.TransactionCategoryIcon);

        if (ukTransactionCategoryIcon != null)
            return new ValueResponse<TransactionCategory> { Status = false, Message = translator.T("Field cannot be duplicated", ["TransactionCategoryIcon"]) };

        var ukTransactionCategoryName = context.TransactionCategorys
            .AsNoTracking()
            .FirstOrDefault(x => x.TransactionCategoryId != transactionCategory.TransactionCategoryId && x.TransactionCategoryName == transactionCategory.TransactionCategoryName);

        if (ukTransactionCategoryName != null)
            return new ValueResponse<TransactionCategory> { Status = false, Message = translator.T("Field cannot be duplicated", ["TransactionCategoryName"]) };

        // CTX: repository-unique-key, do not remove this line
        return new ValueResponse<TransactionCategory> { Status = true, Response = transactionCategory };
    }

    private ValueResponse<TransactionCategory> ValidateParentFK(TransactionCategory transactionCategory)
    {
        // CTX: repository-parent-fk, do not remove this line
        return new ValueResponse<TransactionCategory> { Status = true, Response = transactionCategory };
    }

    private ValueResponse<TransactionCategory> ValidateChildFK(int transactionCategoryId)
    {
        var budgets = context.Budgets
            .AsNoTracking()
            .FirstOrDefault(x => x.TransactionCategoryId == transactionCategoryId);

        if (budgets != null)
            return new ValueResponse<TransactionCategory> { Status = false, Message = translator.T("Entity has child dependencies", ["Budget"]) };

        var transactions = context.Transactions
            .AsNoTracking()
            .FirstOrDefault(x => x.TransactionCategoryId == transactionCategoryId);

        if (transactions != null)
            return new ValueResponse<TransactionCategory> { Status = false, Message = translator.T("Entity has child dependencies", ["Transaction"]) };

        // CTX: repository-child-fk, do not remove this line
        return new ValueResponse<TransactionCategory> { Status = true, Response = new TransactionCategory { TransactionCategoryId = transactionCategoryId } };
    }

    public virtual async Task<int> GetCount()
    {
        return await context.TransactionCategorys.AsNoTracking().CountAsync();
    }

    public virtual IEnumerable<TransactionCategory> GetAllTransactionCategorys()
    {
        return context.TransactionCategorys.AsNoTracking();
    }

    public virtual IEnumerable<TransactionCategory> GetAllTransactionCategorys(int pageSize, int offsetSize)
    {
        return context.TransactionCategorys.AsNoTracking()
            .OrderBy(x => x.TransactionCategoryId)
            .Skip(offsetSize).Take(pageSize);
    }

    public virtual async Task<TransactionCategory?> GetTransactionCategoryById(int transactionCategoryId)
    {
        return await context.TransactionCategorys.FirstOrDefaultAsync(x => x.TransactionCategoryId == transactionCategoryId);
    }

    public virtual async Task<ValueResponse<TransactionCategory>> CreateTransactionCategory(TransactionCategory transactionCategory)
    {
        var ukValidation = ValidateUK(transactionCategory);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(transactionCategory);
        if (!fkValidation.Status)
            return fkValidation;

        var transactionCategoryToUpdate = await GetTransactionCategoryById(transactionCategory.TransactionCategoryId);

        if (transactionCategoryToUpdate != null)
            return await UpdateTransactionCategory(transactionCategory);

        transactionCategory.TransactionCategoryId = 0;
        transactionCategory.Created = DateTime.Now;

        await AddAsync(transactionCategory);
        return new ValueResponse<TransactionCategory>
        {
            Status = true,
            Message = translator.T("Entity created", ["TransactionCategory"]),
            Response = transactionCategory
        };
    }

    public virtual async Task<ValueResponse<TransactionCategory>> UpdateTransactionCategory(TransactionCategory transactionCategory)
    {
        var ukValidation = ValidateUK(transactionCategory);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(transactionCategory);
        if (!fkValidation.Status)
            return fkValidation;

        var transactionCategoryToUpdate = await GetTransactionCategoryById(transactionCategory.TransactionCategoryId);

        if (transactionCategoryToUpdate == null)
            return new ValueResponse<TransactionCategory>
            {
                Status = false,
                Message = translator.T("Entity not found", ["TransactionCategory"])
            };

        transactionCategory.Created = transactionCategoryToUpdate.Created;
        transactionCategory.Modified = DateTime.Now;
        transactionCategoryToUpdate.Bind(transactionCategory);

        await UpdateAsync(transactionCategoryToUpdate);
        return new ValueResponse<TransactionCategory>
        {
            Status = true,
            Message = translator.T("Entity updated", ["TransactionCategory"]),
            Response = transactionCategory
        };
    }

    public virtual async Task<ValueResponse<TransactionCategory>> DeleteTransactionCategory(int transactionCategoryId)
    {
        var fkValidation = ValidateChildFK(transactionCategoryId);
        if (!fkValidation.Status)
            return fkValidation;

        var transactionCategory = await GetTransactionCategoryById(transactionCategoryId);

        if (transactionCategory == null)
            return new ValueResponse<TransactionCategory>
            {
                Status = false,
                Message = translator.T("Entity not found", ["TransactionCategory"])
            };

        await DeleteAsync(transactionCategory);
        return new ValueResponse<TransactionCategory>
        {
            Status = true,
            Message = translator.T("Entity deleted", ["TransactionCategory"]),
            Response = transactionCategory
        };
    }

    public virtual IEnumerable<TransactionCategory> SearchTransactionCategorys(string search)
    {
        return context.TransactionCategorys
            .AsNoTracking()
            .Where(x =>
                x.TransactionCategoryName.Contains(search) ||
                x.TransactionCategoryDescription.Contains(search) ||
                x.TransactionCategoryIcon.Contains(search) ||
                x.TransactionCategoryColor.Contains(search) ||
                // CTX: repository-search, do not remove this line
                false
            );
    }

    public virtual IEnumerable<TransactionCategory> GetTransactionCategorysByAttributes(TransactionCategory transactionCategory)
    {
        var queryableTransactionCategorys = context.TransactionCategorys.AsNoTracking()
            .AsQueryable();

        var isByAttributesFilter = false;

        if (transactionCategory.TransactionCategoryId > 0)
        {
            queryableTransactionCategorys = queryableTransactionCategorys.Where(x => x.TransactionCategoryId == transactionCategory.TransactionCategoryId);
            isByAttributesFilter = true;
        }

        if (!string.IsNullOrEmpty(transactionCategory.TransactionCategoryName))
        {
            queryableTransactionCategorys = queryableTransactionCategorys.Where(x => x.TransactionCategoryName == transactionCategory.TransactionCategoryName);
            isByAttributesFilter = true;
        }

        if (!string.IsNullOrEmpty(transactionCategory.TransactionCategoryDescription))
        {
            queryableTransactionCategorys = queryableTransactionCategorys.Where(x => x.TransactionCategoryDescription == transactionCategory.TransactionCategoryDescription);
            isByAttributesFilter = true;
        }

        if (!string.IsNullOrEmpty(transactionCategory.TransactionCategoryIcon))
        {
            queryableTransactionCategorys = queryableTransactionCategorys.Where(x => x.TransactionCategoryIcon == transactionCategory.TransactionCategoryIcon);
            isByAttributesFilter = true;
        }

        if (!string.IsNullOrEmpty(transactionCategory.TransactionCategoryColor))
        {
            queryableTransactionCategorys = queryableTransactionCategorys.Where(x => x.TransactionCategoryColor == transactionCategory.TransactionCategoryColor);
            isByAttributesFilter = true;
        }

        // CTX: repository-attribute, do not remove this line

        return isByAttributesFilter ? queryableTransactionCategorys : [];
    }
}