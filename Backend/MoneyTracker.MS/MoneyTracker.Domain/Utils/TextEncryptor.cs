namespace MoneyTracker.Domain.Utils;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

[ExcludeFromCodeCoverage]
public static class TextEncryptor
{
    public static string Encrypt(string plainText, string key, string iv)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(key);
        aesAlg.IV = Encoding.UTF8.GetBytes(iv);

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