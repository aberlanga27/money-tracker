namespace MoneyTracker.Infrastructure.Repositories;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Extensions;
using MoneyTracker.Domain.Interfaces;

public class TransactionTypeRepository(
    MoneyTrackerContext context,
    ILocalizationProvider translator
) : Repository<TransactionType>(context), ITransactionTypeRepository
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);

    private ValueResponse<TransactionType> ValidateUK(TransactionType transactionType)
    {
        var ukTransactionTypeName = context.TransactionTypes
            .AsNoTracking()
            .FirstOrDefault(x => x.TransactionTypeId != transactionType.TransactionTypeId && x.TransactionTypeName == transactionType.TransactionTypeName);

        if (ukTransactionTypeName != null)
            return new ValueResponse<TransactionType> { Status = false, Message = translator.T("Field cannot be duplicated", ["TransactionTypeName"]) };

        // CTX: repository-unique-key, do not remove this line
        return new ValueResponse<TransactionType> { Status = true, Response = transactionType };
    }

    private ValueResponse<TransactionType> ValidateParentFK(TransactionType transactionType)
    {
        // CTX: repository-parent-fk, do not remove this line
        return new ValueResponse<TransactionType> { Status = true, Response = transactionType };
    }

    private ValueResponse<TransactionType> ValidateChildFK(int transactionTypeId)
    {
        var transactions = context.Transactions
            .AsNoTracking()
            .FirstOrDefault(x => x.TransactionTypeId == transactionTypeId);

        if (transactions != null)
            return new ValueResponse<TransactionType> { Status = false, Message = translator.T("Entity has child dependencies", ["Transaction"]) };

        // CTX: repository-child-fk, do not remove this line
        return new ValueResponse<TransactionType> { Status = true, Response = new TransactionType { TransactionTypeId = transactionTypeId } };
    }

    public virtual async Task<int> GetCount()
    {
        return await context.TransactionTypes.AsNoTracking().CountAsync();
    }

    public virtual IEnumerable<TransactionType> GetAllTransactionTypes()
    {
        return context.TransactionTypes.AsNoTracking();
    }

    public virtual IEnumerable<TransactionType> GetAllTransactionTypes(int pageSize, int offsetSize)
    {
        return context.TransactionTypes.AsNoTracking()
            .OrderBy(x => x.TransactionTypeId)
            .Skip(offsetSize).Take(pageSize);
    }

    public virtual async Task<TransactionType?> GetTransactionTypeById(int transactionTypeId)
    {
        return await context.TransactionTypes.FirstOrDefaultAsync(x => x.TransactionTypeId == transactionTypeId);
    }

    public virtual async Task<ValueResponse<TransactionType>> CreateTransactionType(TransactionType transactionType)
    {
        var ukValidation = ValidateUK(transactionType);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(transactionType);
        if (!fkValidation.Status)
            return fkValidation;

        var transactionTypeToUpdate = await GetTransactionTypeById(transactionType.TransactionTypeId);

        if (transactionTypeToUpdate != null)
            return await UpdateTransactionType(transactionType);

        transactionType.TransactionTypeId = 0;
        transactionType.Created = DateTime.Now;

        await AddAsync(transactionType);
        return new ValueResponse<TransactionType>
        {
            Status = true,
            Message = translator.T("Entity created", ["TransactionType"]),
            Response = transactionType
        };
    }

    public virtual async Task<ValueResponse<TransactionType>> UpdateTransactionType(TransactionType transactionType)
    {
        var ukValidation = ValidateUK(transactionType);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(transactionType);
        if (!fkValidation.Status)
            return fkValidation;

        var transactionTypeToUpdate = await GetTransactionTypeById(transactionType.TransactionTypeId);

        if (transactionTypeToUpdate == null)
            return new ValueResponse<TransactionType>
            {
                Status = false,
                Message = translator.T("Entity not found", ["TransactionType"])
            };

        transactionType.Created = transactionTypeToUpdate.Created;
        transactionType.Modified = DateTime.Now;
        transactionTypeToUpdate.Bind(transactionType);

        await UpdateAsync(transactionTypeToUpdate);
        return new ValueResponse<TransactionType>
        {
            Status = true,
            Message = translator.T("Entity updated", ["TransactionType"]),
            Response = transactionType
        };
    }

    public virtual async Task<ValueResponse<TransactionType>> DeleteTransactionType(int transactionTypeId)
    {
        var fkValidation = ValidateChildFK(transactionTypeId);
        if (!fkValidation.Status)
            return fkValidation;

        var transactionType = await GetTransactionTypeById(transactionTypeId);

        if (transactionType == null)
            return new ValueResponse<TransactionType>
            {
                Status = false,
                Message = translator.T("Entity not found", ["TransactionType"])
            };

        await DeleteAsync(transactionType);
        return new ValueResponse<TransactionType>
        {
            Status = true,
            Message = translator.T("Entity deleted", ["TransactionType"]),
            Response = transactionType
        };
    }

    public virtual IEnumerable<TransactionType> SearchTransactionTypes(string search)
    {
        return context.TransactionTypes
            .AsNoTracking()
            .Where(x =>
                x.TransactionTypeName.Contains(search) ||
                x.TransactionTypeDescription.Contains(search) ||
                // CTX: repository-search, do not remove this line
                false
            );
    }

    public virtual IEnumerable<TransactionType> GetTransactionTypesByAttributes(TransactionType transactionType)
    {
        var queryableTransactionTypes = context.TransactionTypes.AsNoTracking()
            .AsQueryable();

        var isByAttributesFilter = false;

        if (transactionType.TransactionTypeId > 0)
        {
            queryableTransactionTypes = queryableTransactionTypes.Where(x => x.TransactionTypeId == transactionType.TransactionTypeId);
            isByAttributesFilter = true;
        }

        if (!string.IsNullOrEmpty(transactionType.TransactionTypeName))
        {
            queryableTransactionTypes = queryableTransactionTypes.Where(x => x.TransactionTypeName == transactionType.TransactionTypeName);
            isByAttributesFilter = true;
        }

        if (!string.IsNullOrEmpty(transactionType.TransactionTypeDescription))
        {
            queryableTransactionTypes = queryableTransactionTypes.Where(x => x.TransactionTypeDescription == transactionType.TransactionTypeDescription);
            isByAttributesFilter = true;
        }

        // CTX: repository-attribute, do not remove this line

        return isByAttributesFilter ? queryableTransactionTypes : [];
    }
}