namespace MoneyTracker.Domain.DTOs;

public partial class BudgetTypeDTO
{
    public int BudgetTypeId { get; set; }
    public string? BudgetTypeName { get; set; }
    public int BudgetTypeDays { get; set; }
}