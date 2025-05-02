namespace MoneyTracker.API.Config;

using System.Diagnostics.CodeAnalysis;
using dotenv.net;
using MoneyTracker.Domain.Entities.Config;

/// <summary>
/// AppSettings configuration
/// </summary>
[ExcludeFromCodeCoverage]
public static class AppSettingsConfiguration
{
    private static void SetEnvironment(this WebApplicationBuilder builder, MoneyTrackerSettings appSettings)
    {
        appSettings.Environment = new()
        {
            Name = builder.Configuration["ASPNETCORE_ENVIRONMENT"],
            DockerConnString = builder.Configuration["DOCKER_CONNSTR"],
            RunningInContainer = bool.TryParse(builder.Configuration["RUNNING_IN_CONTAINER"], out var runningInContainer) ? runningInContainer : null
        };

        appSettings.ConnectionStrings.DefaultConnection = builder.Configuration["DEFAULT_CONNSTR"];

        appSettings.Logging.Seq.ApiKey = builder.Configuration["SEQ_API_KEY"];
        appSettings.Cache.Instance = builder.Configuration["REDIS_ENDPOINT"] ?? string.Empty;

        appSettings.Jwt.Key = builder.Configuration["JWT_KEY"];
        appSettings.Encryptor.Key = builder.Configuration["ENCRYPTOR_KEY"];
        appSettings.Encryptor.IV = builder.Configuration["ENCRYPTOR_IV"];
        appSettings.Encryptor.CertificatePassword = builder.Configuration["ENCRYPTOR_CERTIFICATE_PASSWORD"];
    }

    /// <summary>
    /// Add the app settings configuration
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MoneyTrackerSettings AddAppSettingsConfiguration(this WebApplicationBuilder builder)
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
        builder.SetEnvironment(appSettings);
        builder.Services.AddSingleton(appSettings);

        return appSettings;
    }
}