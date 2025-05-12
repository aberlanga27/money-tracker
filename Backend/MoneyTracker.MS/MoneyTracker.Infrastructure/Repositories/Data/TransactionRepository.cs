namespace MoneyTracker.Infrastructure.Repositories;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Extensions;
using MoneyTracker.Domain.Interfaces;

public class TransactionRepository(
    MoneyTrackerContext context,
    ILocalizationProvider translator
) : Repository<Transaction>(context), ITransactionRepository
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);

    private ValueResponse<Transaction> ValidateUK(Transaction transaction)
    {
        // CTX: repository-unique-key, do not remove this line
        return new ValueResponse<Transaction> { Status = true, Response = transaction };
    }

    private ValueResponse<Transaction> ValidateParentFK(Transaction transaction)
    {
        var bank = context.Banks
            .AsNoTracking()
            .FirstOrDefault(x => x.BankId == transaction.BankId);

        if (bank == null)
            return new ValueResponse<Transaction> { Status = false, Message = translator.T("Entity not found", ["Bank"]) };

        var transactionCategory = context.TransactionCategorys
            .AsNoTracking()
            .FirstOrDefault(x => x.TransactionCategoryId == transaction.TransactionCategoryId);

        if (transactionCategory == null)
            return new ValueResponse<Transaction> { Status = false, Message = translator.T("Entity not found", ["TransactionCategory"]) };

        var transactionType = context.TransactionTypes
            .AsNoTracking()
            .FirstOrDefault(x => x.TransactionTypeId == transaction.TransactionTypeId);

        if (transactionType == null)
            return new ValueResponse<Transaction> { Status = false, Message = translator.T("Entity not found", ["TransactionType"]) };

        // CTX: repository-parent-fk, do not remove this line
        return new ValueResponse<Transaction> { Status = true, Response = transaction };
    }

    private ValueResponse<Transaction> ValidateChildFK(int transactionId)
    {
        // CTX: repository-child-fk, do not remove this line
        return new ValueResponse<Transaction> { Status = true, Response = new Transaction { TransactionId = transactionId } };
    }

    public virtual async Task<int> GetCount()
    {
        return await context.Transactions.AsNoTracking().CountAsync();
    }

    public virtual IEnumerable<Transaction> GetAllTransactions()
    {
        return context.Transactions.AsNoTracking().OrderByDescending(x => x.TransactionDate);
    }

    public virtual IEnumerable<Transaction> GetAllTransactions(int pageSize, int offsetSize)
    {
        return context.Transactions.AsNoTracking()
            .Include(x => x.Bank)
            .Include(x => x.TransactionCategory)
            .Include(x => x.TransactionType)
            .OrderByDescending(x => x.TransactionDate)
            .OrderByDescending(x => x.TransactionId)
            .Skip(offsetSize).Take(pageSize);
    }

    public virtual async Task<Transaction?> GetTransactionById(int transactionId)
    {
        return await context.Transactions.FirstOrDefaultAsync(x => x.TransactionId == transactionId);
    }

    public virtual async Task<ValueResponse<Transaction>> CreateTransaction(Transaction transaction)
    {
        var ukValidation = ValidateUK(transaction);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(transaction);
        if (!fkValidation.Status)
            return fkValidation;

        var transactionToUpdate = await GetTransactionById(transaction.TransactionId);

        if (transactionToUpdate != null)
            return await UpdateTransaction(transaction);

        transaction.TransactionId = 0;
        transaction.Created = DateTime.Now;

        await AddAsync(transaction);
        return new ValueResponse<Transaction>
        {
            Status = true,
            Message = translator.T("Entity created", ["Transaction"]),
            Response = transaction
        };
    }

    public virtual async Task<ValueResponse<Transaction>> UpdateTransaction(Transaction transaction)
    {
        var ukValidation = ValidateUK(transaction);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(transaction);
        if (!fkValidation.Status)
            return fkValidation;

        var transactionToUpdate = await GetTransactionById(transaction.TransactionId);

        if (transactionToUpdate == null)
            return new ValueResponse<Transaction>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Transaction"])
            };

        transaction.Created = transactionToUpdate.Created;
        transaction.Modified = DateTime.Now;
        transactionToUpdate.Bind(transaction);

        await UpdateAsync(transactionToUpdate);
        return new ValueResponse<Transaction>
        {
            Status = true,
            Message = translator.T("Entity updated", ["Transaction"]),
            Response = transaction
        };
    }

    public virtual async Task<ValueResponse<Transaction>> DeleteTransaction(int transactionId)
    {
        var fkValidation = ValidateChildFK(transactionId);
        if (!fkValidation.Status)
            return fkValidation;

        var transaction = await GetTransactionById(transactionId);

        if (transaction == null)
            return new ValueResponse<Transaction>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Transaction"])
            };

        await DeleteAsync(transaction);
        return new ValueResponse<Transaction>
        {
            Status = true,
            Message = translator.T("Entity deleted", ["Transaction"]),
            Response = transaction
        };
    }

    public virtual IEnumerable<Transaction> SearchTransactions(string search)
    {
        return context.Transactions
            .AsNoTracking()
            .Include(x => x.Bank)
            .Include(x => x.TransactionCategory)
            .Include(x => x.TransactionType)
            .OrderByDescending(x => x.TransactionDate)
            .OrderByDescending(x => x.TransactionId)
            .Where(x =>
                x.TransactionDescription.Contains(search) ||
                // CTX: repository-search, do not remove this line
                false
            );
    }

    public virtual IEnumerable<Transaction> GetTransactionsByAttributes(Transaction transaction)
    {
        var queryableTransactions = context.Transactions.AsNoTracking()
            .Include(x => x.Bank)
            .Include(x => x.TransactionCategory)
            .Include(x => x.TransactionType)
            .OrderByDescending(x => x.TransactionDate)
            .OrderByDescending(x => x.TransactionId)
            .AsQueryable();

        var isByAttributesFilter = false;

        if (transaction.TransactionId > 0)
        {
            queryableTransactions = queryableTransactions.Where(x => x.TransactionId == transaction.TransactionId);
            isByAttributesFilter = true;
        }

        if (transaction.TransactionCategoryId > 0)
        {
            queryableTransactions = queryableTransactions.Where(x => x.TransactionCategoryId == transaction.TransactionCategoryId);
            isByAttributesFilter = true;
        }

        if (transaction.TransactionTypeId > 0)
        {
            queryableTransactions = queryableTransactions.Where(x => x.TransactionTypeId == transaction.TransactionTypeId);
            isByAttributesFilter = true;
        }

        if (transaction.BankId > 0)
        {
            queryableTransactions = queryableTransactions.Where(x => x.BankId == transaction.BankId);
            isByAttributesFilter = true;
        }

        if (transaction.TransactionAmount > 0)
        {
            queryableTransactions = queryableTransactions.Where(x => x.TransactionAmount == transaction.TransactionAmount);
            isByAttributesFilter = true;
        }

        if (!string.IsNullOrEmpty(transaction.TransactionDescription))
        {
            queryableTransactions = queryableTransactions.Where(x => x.TransactionDescription == transaction.TransactionDescription);
            isByAttributesFilter = true;
        }

        // CTX: repository-attribute, do not remove this line

        return isByAttributesFilter ? queryableTransactions : [];
    }

    // ...

    public virtual IEnumerable<TransactionsGroupedByCategory> GetTransactionsGroupedByCategory(DateTime startDate, DateTime endDate)
    {
        return context.Transactions
            .AsNoTracking()
            .Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate)
            .Include(x => x.TransactionCategory)
            .GroupBy(x => x.TransactionCategory)
            .Select(x => new TransactionsGroupedByCategory
            {
                TransactionCategory = x.Key,
                TotalAmount = x.Sum(y => y.TransactionAmount)
            });
    }

    public virtual IEnumerable<TransactionsGroupedByBank> GetTransactionsGroupedByBank(DateTime startDate, DateTime endDate)
    {
        return context.Transactions
            .AsNoTracking()
            .Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate)
            .Include(x => x.Bank)
            .GroupBy(x => x.Bank)
            .Select(x => new TransactionsGroupedByBank
            {
                Bank = x.Key,
                TotalAmount = x.Sum(y => y.TransactionAmount)
            });
    }
}