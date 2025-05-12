namespace MoneyTracker.Domain.Entities;

public partial class TransactionsGroupedByBank
{
    public Bank Bank { get; set; } = null!;
    public decimal TotalAmount { get; set; }
}