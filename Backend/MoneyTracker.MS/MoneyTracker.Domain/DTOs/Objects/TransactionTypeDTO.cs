namespace MoneyTracker.Domain.DTOs;

public partial class TransactionTypeDTO
{
    public int TransactionTypeId { get; set; }
    public string? TransactionTypeName { get; set; }
    public string? TransactionTypeDescription { get; set; }
}