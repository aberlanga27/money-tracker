namespace MoneyTracker.Domain.Mappers;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using Riok.Mapperly.Abstractions;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
[ExcludeFromCodeCoverage]
public partial class TransactionTypeMapper
{
    public partial TransactionTypeDTO Map(TransactionType entity);
    public partial TransactionType Map(TransactionTypeDTO dto);
    public partial IEnumerable<TransactionTypeDTO> Map(IEnumerable<TransactionType> entities);
    public partial IEnumerable<TransactionType> Map(IEnumerable<TransactionTypeDTO> dtos);
}