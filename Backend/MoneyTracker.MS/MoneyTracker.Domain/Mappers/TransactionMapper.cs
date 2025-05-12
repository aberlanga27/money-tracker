namespace MoneyTracker.Domain.Mappers;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using Riok.Mapperly.Abstractions;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
[ExcludeFromCodeCoverage]
public partial class TransactionMapper
{
    public partial TransactionDTO Map(Transaction entity);
    public partial Transaction Map(TransactionDTO dto);
    public partial IEnumerable<TransactionDTO> Map(IEnumerable<Transaction> entities);
    public partial IEnumerable<Transaction> Map(IEnumerable<TransactionDTO> dtos);

    // ...

    public partial TransactionAttributesDTO MapToAttributes(Transaction entity);

    [UserMapping(Default = true)]
    public TransactionAttributesDTO MapAttributes(Transaction entity)
    {
        var dto = MapToAttributes(entity);
        dto.TransactionCategoryName = entity.TransactionCategory?.TransactionCategoryName;
        dto.TransactionTypeName = entity.TransactionType?.TransactionTypeName;
        dto.BankName = entity.Bank?.BankName;

        return dto;
    }

    public partial IEnumerable<TransactionAttributesDTO> MapAttributes(IEnumerable<Transaction> entities);

    // ...

    [MapProperty(nameof(TransactionsGroupedByCategory.TransactionCategory.TransactionCategoryId), nameof(TransactionsGroupedByCategoryDTO.TransactionCategoryId))]
    [MapProperty(nameof(TransactionsGroupedByCategory.TransactionCategory.TransactionCategoryName), nameof(TransactionsGroupedByCategoryDTO.TransactionCategoryName))]
    [MapProperty(nameof(TransactionsGroupedByCategory.TransactionCategory.TransactionCategoryIcon), nameof(TransactionsGroupedByCategoryDTO.TransactionCategoryIcon))]
    [MapProperty(nameof(TransactionsGroupedByCategory.TransactionCategory.TransactionCategoryColor), nameof(TransactionsGroupedByCategoryDTO.TransactionCategoryColor))]
    public partial TransactionsGroupedByCategoryDTO Map(TransactionsGroupedByCategory entity);
    public partial IEnumerable<TransactionsGroupedByCategoryDTO> Map(IEnumerable<TransactionsGroupedByCategory> entities);

    // ...

    [MapProperty(nameof(TransactionsGroupedByBank.Bank.BankId), nameof(TransactionsGroupedByBankDTO.BankId))]
    [MapProperty(nameof(TransactionsGroupedByBank.Bank.BankName), nameof(TransactionsGroupedByBankDTO.BankName))]
    public partial TransactionsGroupedByBankDTO Map(TransactionsGroupedByBank entity);
    public partial IEnumerable<TransactionsGroupedByBankDTO> Map(IEnumerable<TransactionsGroupedByBank> entities);
}