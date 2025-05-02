namespace MoneyTracker.Domain.Entities.Config;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public partial class ApiConfiguration
{
    public string? Language { get; set; }
}