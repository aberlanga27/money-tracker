namespace MoneyTracker.Domain.Entities.Config;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public partial class MoneyTrackerSettings
{
    public class EnvironmentModel
    {
        public string? Name { get; set; }
        public string? DockerConnString { get; set; }
        public bool? RunningInContainer { get; set; }
    }

    public class LogLevelModel
    {
        public string Default { get; set; } = string.Empty;
        public string Microsoft { get; set; } = string.Empty;
    }

    public class SeqModel
    {
        public string BaseAddress { get; set; } = string.Empty;
        public string? ApiKey { get; set; }
    }

    public class LoggingModel
    {
        public LogLevelModel LogLevel { get; set; } = new();
        public SeqModel Seq { get; set; } = new();
    }

    public class ConnectionStringsModel
    {
        public string? DefaultConnection { get; set; }
    }

    public class EncryptorModel
    {
        public string? Key { get; set; }
        public string? IV { get; set; }
        public string? CertificatePassword { get; set; }
    }

    public class JwtModel
    {
        public string? Key { get; set; }
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; }
    }

    public class WorkersModel
    {
        public int SampleInterval { get; set; }
    }

    public class SmtpModel
    {
        public string? Server { get; set; }
        public int? Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class LocalizationModel
    {
        public string Default { get; set; } = string.Empty;
        public string[] Supported { get; set; } = [];
    }

    public class PaginationModel
    {
        public int DefaultPageSize { get; set; } = 10;
        public int MaxPageSize { get; set; } = 100;
    }

    public class CacheModel
    {
        public string Instance { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; }
    }

    public EnvironmentModel Environment { get; set; } = new();
    public LoggingModel Logging { get; set; } = new();
    public string? AllowedHosts { get; set; }
    public string[]? AllowedOrigins { get; set; }
    public ConnectionStringsModel ConnectionStrings { get; set; } = new();
    public EncryptorModel Encryptor { get; set; } = new();
    public JwtModel Jwt { get; set; } = new();
    public WorkersModel Workers { get; set; } = new();
    public SmtpModel Smtp { get; set; } = new();
    public LocalizationModel Localization { get; set; } = new();
    public PaginationModel Pagination { get; set; } = new();
    public CacheModel Cache { get; set; } = new();
}