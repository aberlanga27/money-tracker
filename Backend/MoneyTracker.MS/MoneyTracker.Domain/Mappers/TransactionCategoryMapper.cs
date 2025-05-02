namespace MoneyTracker.Domain.Mappers;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using Riok.Mapperly.Abstractions;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
[ExcludeFromCodeCoverage]
public partial class TransactionCategoryMapper
{
    public partial TransactionCategoryDTO Map(TransactionCategory entity);
    public partial TransactionCategory Map(TransactionCategoryDTO dto);
    public partial IEnumerable<TransactionCategoryDTO> Map(IEnumerable<TransactionCategory> entities);
    public partial IEnumerable<TransactionCategory> Map(IEnumerable<TransactionCategoryDTO> dtos);
}