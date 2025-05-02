namespace MoneyTracker.Domain.DTOs;

public partial class TransactionCategoryDTO
{
    public int TransactionCategoryId { get; set; }
    public string? TransactionCategoryName { get; set; }
    public string? TransactionCategoryDescription { get; set; }
    public string? TransactionCategoryIcon { get; set; }
    public string? TransactionCategoryColor { get; set; }
}