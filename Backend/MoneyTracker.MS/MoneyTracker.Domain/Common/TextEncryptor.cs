namespace MoneyTracker.Domain.Common;

using System.Security.Cryptography;
using System.Text;
using Ardalis.GuardClauses;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;

public class TextEncryptor(
    MoneyTrackerSettings appSettings
) : ITextEncryptor
{
    private readonly string KEY = Guard.Against.Null(appSettings.Encryptor.Key);
    private readonly string IV = Guard.Against.Null(appSettings.Encryptor.IV);

    public string Encrypt(string plainText)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(KEY);
        aesAlg.IV = Encoding.UTF8.GetBytes(IV);

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(plainText);
            swEncrypt.Flush();
        }

        return Convert.ToBase64String(msEncrypt.ToArray());
    }
}