namespace MoneyTracker.Domain.Interfaces;

public interface ITextEncryptor
{
    string Encrypt(string plainText);
}