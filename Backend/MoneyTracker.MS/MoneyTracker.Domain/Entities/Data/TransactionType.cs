namespace MoneyTracker.Domain.Entities;

public partial class TransactionType
{
    public int TransactionTypeId { get; set; }
    public string TransactionTypeName { get; set; } = null!;
    public string TransactionTypeDescription { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = [];
}