namespace MoneyTracker.Domain.Entities;

public partial class EmailPayload
{
    public string[] Recipients { get; set; } = null!;
    public string[]? RecipientsCc { get; set; }
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
}