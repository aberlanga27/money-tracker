namespace MoneyTracker.API;

using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using MoneyTracker.API.Config;
using MoneyTracker.API.Filters;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Infrastructure;

/// <summary>
/// Main class of the API
/// </summary>
[ExcludeFromCodeCoverage]
public class Program
{
    /// <summary>
    /// Main method of the API
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

        var appSettings = builder.AddAppSettingsConfiguration();
        builder.Services.AddSingleton<ApiConfiguration>();

        builder.Services
            .AddControllers(options =>
            {
                options.Filters.Add<HttpGlobalExceptionFilter>();
            })
            .AddNewtonsoftJson();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerConfiguration(appSettings);
        builder.Services.AddAuthorization();
        builder.Services.AddCorsConfiguration(appSettings);

        // SelfSignedCertificate.Create( // See TODO.md to uncomment this line
        //     $"Certs/MoneyTracker.MS.{DateTime.Now.Year}.pfx",
        //     appSettings.Jwt.Issuer,
        //     appSettings.Encryptor.CertificatePassword ?? string.Empty
        // );

        builder.Services
            .AddDatabase(appSettings)
            // .AddPersistentDataProtection(appSettings) // See TODO.md to uncomment this line
            .AddCacheProvider(appSettings) // See TODO.md to improve cache implementation
            .AddProviders()
            .AddCommon()
            .AddRepositories()
            .AddServices()
            .AddMiddlewares()
            //.AddHostedServices() // See TODO.md to uncomment this line
            .AddValidatorsFromAssemblyContaining<Program>();

        // builder.Host.AddLoggingConfiguration(appSettings); // See TODO.md to uncomment this line

        // ...

        var app = builder.Build();

        if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            app.AddSwaggerUI(appSettings);

        app.UseAppMiddlewares();

        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseCors("DefaultPolicy");

        // app.MapSignalRHubs(); // See TODO.md to uncomment this line

        app.Run();
    }
}