namespace MoneyTracker.Domain.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }
    public int TransactionCategoryId { get; set; }
    public int TransactionTypeId { get; set; }
    public int BankId { get; set; }
    public decimal TransactionAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string TransactionDescription { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public virtual Bank Bank { get; set; } = null!;
    public virtual TransactionCategory TransactionCategory { get; set; } = null!;
    public virtual TransactionType TransactionType { get; set; } = null!;
}