namespace MoneyTracker.Domain.Entities;

public partial class Budget
{
    public int BudgetId { get; set; }
    public int TransactionCategoryId { get; set; }
    public int BudgetTypeId { get; set; }
    public decimal BudgetAmount { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public virtual BudgetType BudgetType { get; set; } = null!;
    public virtual TransactionCategory TransactionCategory { get; set; } = null!;
}