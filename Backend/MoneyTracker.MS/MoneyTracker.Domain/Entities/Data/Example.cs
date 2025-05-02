namespace MoneyTracker.Domain.Entities;

public partial class Example
{
    public int ExampleId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Url { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }
}