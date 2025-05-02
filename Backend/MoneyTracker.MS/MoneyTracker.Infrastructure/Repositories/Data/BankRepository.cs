namespace MoneyTracker.Infrastructure.Repositories;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Extensions;
using MoneyTracker.Domain.Interfaces;

public class BankRepository(
    MoneyTrackerContext context,
    ILocalizationProvider translator
) : Repository<Bank>(context), IBankRepository
{
    private readonly ILocalizationProvider translator = Guard.Against.Null(translator);

    private ValueResponse<Bank> ValidateUK(Bank bank)
    {
        var ukBankName = context.Banks
            .AsNoTracking()
            .FirstOrDefault(x => x.BankId != bank.BankId && x.BankName == bank.BankName);

        if (ukBankName != null)
            return new ValueResponse<Bank> { Status = false, Message = translator.T("Field cannot be duplicated", ["BankName"]) };

        // CTX: repository-unique-key, do not remove this line
        return new ValueResponse<Bank> { Status = true, Response = bank };
    }

    private ValueResponse<Bank> ValidateParentFK(Bank bank)
    {
        // CTX: repository-parent-fk, do not remove this line
        return new ValueResponse<Bank> { Status = true, Response = bank };
    }

    private ValueResponse<Bank> ValidateChildFK(int bankId)
    {
        var transactions = context.Transactions
            .AsNoTracking()
            .FirstOrDefault(x => x.BankId == bankId);

        if (transactions != null)
            return new ValueResponse<Bank> { Status = false, Message = translator.T("Entity has child dependencies", ["Transaction"]) };

        // CTX: repository-child-fk, do not remove this line
        return new ValueResponse<Bank> { Status = true, Response = new Bank { BankId = bankId } };
    }

    public virtual async Task<int> GetCount()
    {
        return await context.Banks.AsNoTracking().CountAsync();
    }

    public virtual IEnumerable<Bank> GetAllBanks()
    {
        return context.Banks.AsNoTracking();
    }

    public virtual IEnumerable<Bank> GetAllBanks(int pageSize, int offsetSize)
    {
        return context.Banks.AsNoTracking()
            .OrderBy(x => x.BankId)
            .Skip(offsetSize).Take(pageSize);
    }

    public virtual async Task<Bank?> GetBankById(int bankId)
    {
        return await context.Banks.FirstOrDefaultAsync(x => x.BankId == bankId);
    }

    public virtual async Task<ValueResponse<Bank>> CreateBank(Bank bank)
    {
        var ukValidation = ValidateUK(bank);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(bank);
        if (!fkValidation.Status)
            return fkValidation;

        var bankToUpdate = await GetBankById(bank.BankId);

        if (bankToUpdate != null)
            return await UpdateBank(bank);

        bank.BankId = 0;
        bank.Created = DateTime.Now;

        await AddAsync(bank);
        return new ValueResponse<Bank>
        {
            Status = true,
            Message = translator.T("Entity created", ["Bank"]),
            Response = bank
        };
    }

    public virtual async Task<ValueResponse<Bank>> UpdateBank(Bank bank)
    {
        var ukValidation = ValidateUK(bank);
        if (!ukValidation.Status)
            return ukValidation;

        var fkValidation = ValidateParentFK(bank);
        if (!fkValidation.Status)
            return fkValidation;

        var bankToUpdate = await GetBankById(bank.BankId);

        if (bankToUpdate == null)
            return new ValueResponse<Bank>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Bank"])
            };

        bank.Created = bankToUpdate.Created;
        bank.Modified = DateTime.Now;
        bankToUpdate.Bind(bank);

        await UpdateAsync(bankToUpdate);
        return new ValueResponse<Bank>
        {
            Status = true,
            Message = translator.T("Entity updated", ["Bank"]),
            Response = bank
        };
    }

    public virtual async Task<ValueResponse<Bank>> DeleteBank(int bankId)
    {
        var fkValidation = ValidateChildFK(bankId);
        if (!fkValidation.Status)
            return fkValidation;

        var bank = await GetBankById(bankId);

        if (bank == null)
            return new ValueResponse<Bank>
            {
                Status = false,
                Message = translator.T("Entity not found", ["Bank"])
            };

        await DeleteAsync(bank);
        return new ValueResponse<Bank>
        {
            Status = true,
            Message = translator.T("Entity deleted", ["Bank"]),
            Response = bank
        };
    }

    public virtual IEnumerable<Bank> SearchBanks(string search)
    {
        return context.Banks
            .AsNoTracking()
            .Where(x =>
                x.BankName.Contains(search) ||
                // CTX: repository-search, do not remove this line
                false
            );
    }

    public virtual IEnumerable<Bank> GetBanksByAttributes(Bank bank)
    {
        var queryableBanks = context.Banks.AsNoTracking()
            .AsQueryable();

        var isByAttributesFilter = false;

        if (bank.BankId > 0)
        {
            queryableBanks = queryableBanks.Where(x => x.BankId == bank.BankId);
            isByAttributesFilter = true;
        }

        if (!string.IsNullOrEmpty(bank.BankName))
        {
            queryableBanks = queryableBanks.Where(x => x.BankName == bank.BankName);
            isByAttributesFilter = true;
        }

        // CTX: repository-attribute, do not remove this line

        return isByAttributesFilter ? queryableBanks : [];
    }
}