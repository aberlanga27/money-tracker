namespace MoneyTracker.Domain.Entities;

public partial class BudgetType
{
    public int BudgetTypeId { get; set; }
    public string BudgetTypeName { get; set; } = null!;
    public int BudgetTypeDays { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = [];
}