namespace MoneyTracker.Domain.DTOs;

public partial class TransactionsGroupedByBankDTO
{
    public int BankId { get; set; }
    public string BankName { get; set; } = null!;
    public decimal TotalAmount { get; set; }
}
