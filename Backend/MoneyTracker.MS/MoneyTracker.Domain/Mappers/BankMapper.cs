namespace MoneyTracker.Domain.Mappers;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using Riok.Mapperly.Abstractions;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
[ExcludeFromCodeCoverage]
public partial class BankMapper
{
    public partial BankDTO Map(Bank entity);
    public partial Bank Map(BankDTO dto);
    public partial IEnumerable<BankDTO> Map(IEnumerable<Bank> entities);
    public partial IEnumerable<Bank> Map(IEnumerable<BankDTO> dtos);
}