namespace MoneyTracker.Domain.Interfaces;

public interface IJwtTools
{

    public string? GenerateToken(Dictionary<string, string>? claims = null);
}