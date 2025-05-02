namespace MoneyTracker.Domain.DTOs;

public partial class BudgetDTO
{
    public int BudgetId { get; set; }
    public int TransactionCategoryId { get; set; }
    public int BudgetTypeId { get; set; }
    public decimal BudgetAmount { get; set; }
}

public partial class BudgetAttributesDTO : BudgetDTO
{
    public string? TransactionCategoryName { get; set; }
    public string? BudgetTypeName { get; set; }
}
