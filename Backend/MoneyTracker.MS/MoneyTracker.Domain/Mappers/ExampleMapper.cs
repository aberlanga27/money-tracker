namespace MoneyTracker.Domain.Mappers;

using System.Diagnostics.CodeAnalysis;
using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using Riok.Mapperly.Abstractions;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
[ExcludeFromCodeCoverage]
public partial class ExampleMapper
{
    public partial ExampleDTO Map(Example entity);
    public partial Example Map(ExampleDTO dto);
    public partial IEnumerable<ExampleDTO> Map(IEnumerable<Example> entities);
    public partial IEnumerable<Example> Map(IEnumerable<ExampleDTO> dtos);
}