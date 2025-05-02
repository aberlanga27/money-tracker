namespace MoneyTracker.Domain.Mappers;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using Riok.Mapperly.Abstractions;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
[ExcludeFromCodeCoverage]
public partial class BudgetTypeMapper
{
    public partial BudgetTypeDTO Map(BudgetType entity);
    public partial BudgetType Map(BudgetTypeDTO dto);
    public partial IEnumerable<BudgetTypeDTO> Map(IEnumerable<BudgetType> entities);
    public partial IEnumerable<BudgetType> Map(IEnumerable<BudgetTypeDTO> dtos);
}