namespace MoneyTracker.Domain.DTOs;

public partial class TransactionsGroupedByCategoryDTO
{
    public int TransactionCategoryId { get; set; }
    public string TransactionCategoryName { get; set; } = null!;
    public string TransactionCategoryIcon { get; set; } = null!;
    public string TransactionCategoryColor { get; set; } = null!;
    public decimal TotalAmount { get; set; }
}
