namespace MoneyTracker.Domain.Entities;

public partial class TransactionCategory
{
    public int TransactionCategoryId { get; set; }
    public string TransactionCategoryName { get; set; } = null!;
    public string TransactionCategoryDescription { get; set; } = null!;
    public string TransactionCategoryIcon { get; set; } = null!;
    public string TransactionCategoryColor { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = [];
    public virtual ICollection<Transaction> Transactions { get; set; } = [];
}