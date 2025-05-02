namespace MoneyTracker.Domain.Utils;

using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

[ExcludeFromCodeCoverage]
public static class SelfSignedCertificate
{
    public static void Create(string certPath, string subject, string password)
    {
        if (File.Exists(certPath))
            return;

        using var rsa = RSA.Create(2048);

        var certRequest = new CertificateRequest(
            $"CN={subject}",
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );

        certRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));

        var cert = certRequest.CreateSelfSigned(
            new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
            new DateTimeOffset(DateTime.UtcNow.AddYears(1))
        );

        var securePassword = new SecureString();
        foreach (var c in password)
            securePassword.AppendChar(c);

        var certData = cert.Export(X509ContentType.Pfx, securePassword);
        File.WriteAllBytes(certPath, certData);
    }
}