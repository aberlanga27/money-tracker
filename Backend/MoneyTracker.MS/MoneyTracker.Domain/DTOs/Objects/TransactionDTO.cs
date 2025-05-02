namespace MoneyTracker.Domain.DTOs;

public partial class TransactionDTO
{
    public int TransactionId { get; set; }
    public int TransactionCategoryId { get; set; }
    public int TransactionTypeId { get; set; }
    public int BankId { get; set; }
    public decimal TransactionAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? TransactionDescription { get; set; }
}

public partial class TransactionAttributesDTO : TransactionDTO
{
    public string? TransactionCategoryName { get; set; }
    public string? TransactionTypeName { get; set; }
    public string? BankName { get; set; }
}