namespace MoneyTracker.Domain.Entities;

public partial class Bank
{
    public int BankId { get; set; }
    public string BankName { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = [];
}