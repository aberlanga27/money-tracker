namespace MoneyTracker.Domain.Mappers;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using Riok.Mapperly.Abstractions;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
[ExcludeFromCodeCoverage]
public partial class BudgetMapper
{
    public partial BudgetDTO Map(Budget entity);
    public partial Budget Map(BudgetDTO dto);
    public partial IEnumerable<BudgetDTO> Map(IEnumerable<Budget> entities);
    public partial IEnumerable<Budget> Map(IEnumerable<BudgetDTO> dtos);

    // ...

    public partial BudgetAttributesDTO MapToAttributes(Budget entity);

    [UserMapping(Default = true)]
    public BudgetAttributesDTO MapAttributes(Budget entity)
    {
        var dto = MapToAttributes(entity);
        dto.TransactionCategoryName = entity.TransactionCategory?.TransactionCategoryName;
        dto.BudgetTypeName = entity.BudgetType?.BudgetTypeName;

        return dto;
    }

    public partial IEnumerable<BudgetAttributesDTO> MapAttributes(IEnumerable<Budget> entities);
}