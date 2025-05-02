namespace MoneyTracker.Domain.Entities;

public partial class TransactionsGroupedByCategory
{
    public TransactionCategory TransactionCategory { get; set; } = null!;
    public decimal TotalAmount { get; set; }
}