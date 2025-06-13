namespace MoneyTracker.Domain.Entities.Config;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class SecretManagerAuth
{
    public string AccessToken { get; set; } = string.Empty;
}

[ExcludeFromCodeCoverage]
public class SecretManagerSecret
{
    public string SecretKey { get; set; } = string.Empty;
    public string SecretValue { get; set; } = string.Empty;
}

[ExcludeFromCodeCoverage]
public class SecretManagerSecretValue
{
    public SecretManagerSecret Secret { get; set; } = null!;
}