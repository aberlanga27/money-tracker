namespace MoneyTracker.API.Config;

using System.Diagnostics.CodeAnalysis;
using dotenv.net;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Common;

/// <summary>
/// AppSettings configuration
/// </summary>
[ExcludeFromCodeCoverage]
public static class AppSettingsConfiguration
{
    /// <summary>
    /// Add the app settings configuration
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static async Task<MoneyTrackerSettings> AddAppSettingsConfiguration(this WebApplicationBuilder builder)
    {
        DotEnv.Load(new DotEnvOptions(true, ["../../../.env"]));

        var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"];

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        builder.Configuration.AddConfiguration(configuration);

        var appSettings = new MoneyTrackerSettings();
        builder.Configuration.Bind(appSettings);

        await SetEnvironment(builder, appSettings);

        builder.Services.AddSingleton(appSettings);

        return appSettings;
    }

    private static async Task SetEnvironment(WebApplicationBuilder builder, MoneyTrackerSettings appSettings)
    {
        if (
            !string.IsNullOrWhiteSpace(builder.Configuration["SECRETS_API_URL"]) &&
            !string.IsNullOrWhiteSpace(builder.Configuration["SECRETS_WORKSPACE_ID"]) &&
            !string.IsNullOrWhiteSpace(builder.Configuration["SECRETS_CLIENT_SECRET"]) &&
            !string.IsNullOrWhiteSpace(builder.Configuration["SECRETS_CLIENT_ID"]))
        {
            await SetEnvironmentFromSecrets(builder, appSettings);
            return;
        }

        SetEnvironmentFromLocalSettings(builder, appSettings);
    }

    private static async Task SetEnvironmentFromSecrets(this WebApplicationBuilder builder, MoneyTrackerSettings appSettings)
    {
        var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development";
        var secretsApiUrl = builder.Configuration["SECRETS_API_URL"];
        var secretsWorkspaceId = builder.Configuration["SECRETS_WORKSPACE_ID"];
        var secretsClientSecret = builder.Configuration["SECRETS_CLIENT_SECRET"];
        var secretsClientId = builder.Configuration["SECRETS_CLIENT_ID"];

        var secretManager = new SecretManager(secretsApiUrl!, secretsClientSecret!, secretsClientId!, secretsWorkspaceId!, environment.ToLowerInvariant());

        appSettings.Environment = new()
        {
            Name = environment,
            DockerConnString = await secretManager.GetSecret("DOCKER_CONN_STRING"),
            RunningInContainer = bool.TryParse(builder.Configuration["RUNNING_IN_CONTAINER"], out var runningInContainer) ? runningInContainer : null
        };

        appSettings.ConnectionStrings.DefaultConnection = await secretManager.GetSecret("DEFAULT_CONN_STRING");
        appSettings.Logging.Seq.ApiKey = await secretManager.GetSecret("SEQ_API_KEY");
        appSettings.Cache.Instance = await secretManager.GetSecret("REDIS_ENDPOINT");
        appSettings.Jwt.Key = await secretManager.GetSecret("JWT_KEY");
        appSettings.Encryptor.Key = await secretManager.GetSecret("ENCRYPTOR_KEY");
        appSettings.Encryptor.IV = await secretManager.GetSecret("ENCRYPTOR_IV");
        appSettings.Encryptor.CertificatePassword = await secretManager.GetSecret("ENCRYPTOR_CERTIFICATE_PASSWORD");
    }

    private static void SetEnvironmentFromLocalSettings(this WebApplicationBuilder builder, MoneyTrackerSettings appSettings)
    {
        var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development";

        appSettings.Environment = new()
        {
            Name = environment,
            DockerConnString = builder.Configuration["DOCKER_CONN_STRING"],
            RunningInContainer = bool.TryParse(builder.Configuration["RUNNING_IN_CONTAINER"], out var runningInContainer) ? runningInContainer : null
        };

        appSettings.ConnectionStrings.DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
        appSettings.Logging.Seq.ApiKey = builder.Configuration["SEQ_API_KEY"];
        appSettings.Cache.Instance = builder.Configuration["REDIS_ENDPOINT"] ?? string.Empty;
        appSettings.Jwt.Key = builder.Configuration["JWT_KEY"];
        appSettings.Encryptor.Key = builder.Configuration["ENCRYPTOR_KEY"];
        appSettings.Encryptor.IV = builder.Configuration["ENCRYPTOR_IV"];
        appSettings.Encryptor.CertificatePassword = builder.Configuration["ENCRYPTOR_CERTIFICATE_PASSWORD"];
    }
}