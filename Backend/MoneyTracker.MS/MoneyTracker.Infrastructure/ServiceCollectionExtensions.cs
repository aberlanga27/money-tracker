namespace MoneyTracker.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Domain.Common;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Services;
using MoneyTracker.Infrastructure.Providers;
using MoneyTracker.Infrastructure.Repositories;
using StackExchange.Redis;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, MoneyTrackerSettings appSettings)
    {
        services.AddDbContext<MoneyTrackerContext>(options =>
        {
            if (appSettings.Environment?.RunningInContainer == true && appSettings.Environment?.DockerConnString != null)
                options.UseSqlServer(appSettings.Environment?.DockerConnString);
            else
                options.UseSqlServer(appSettings.ConnectionStrings?.DefaultConnection);
        });

        services.AddScoped<Func<MoneyTrackerContext>>((provider) => () => provider?.GetService<MoneyTrackerContext>() ?? new MoneyTrackerContext());
        return services;
    }

    public static IServiceCollection AddPersistentDataProtection(this IServiceCollection services, MoneyTrackerSettings appSettings)
    {
        services.AddDataProtection()
            .SetApplicationName("MoneyTracker.MS")
            .PersistKeysToDbContext<MoneyTrackerContext>()
            .ProtectKeysWithCertificate(new X509Certificate2($"Certs/MoneyTracker.MS.{DateTime.Now.Year}.pfx", appSettings.Encryptor?.CertificatePassword ?? string.Empty))
        ;

        return services;
    }

    public static IServiceCollection AddCacheProvider(this IServiceCollection services, MoneyTrackerSettings appSettings)
    {
        if (string.IsNullOrWhiteSpace(appSettings.Cache.Instance))
            return services
                .AddDistributedMemoryCache()
                .AddSingleton<ICacheProvider, LocalCacheProvider>()
            ;

        return services
            .AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(appSettings.Cache.Instance))
            .AddSingleton<ICacheProvider, RedisCacheProvider>()
        ;
    }

    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        return services
            .AddSingleton<IEmailProvider, EmailProvider>()
            .AddSingleton<IFileProvider, FileProvider>()
            .AddSingleton<ILocalizationProvider, LocalizationProvider>()
        ;
    }

    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        return services
            .AddSingleton<ITextEncryptor, TextEncryptor>()
            .AddSingleton<IJwtTools, JwtTools>()
        ;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IExampleRepository, ExampleRepository>()
            .AddScoped<IBankRepository, BankRepository>()
            .AddScoped<IBudgetRepository, BudgetRepository>()
            .AddScoped<IBudgetTypeRepository, BudgetTypeRepository>()
            .AddScoped<ITransactionRepository, TransactionRepository>()
            .AddScoped<ITransactionCategoryRepository, TransactionCategoryRepository>()
            .AddScoped<ITransactionTypeRepository, TransactionTypeRepository>()
            // CTX: repository, do not remove this line
            // ...
            .AddScoped<IMoneyTrackerRepository, MoneyTrackerRepository>()
        ;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IExampleService, ExampleService>()
            .AddScoped<IBankService, BankService>()
            .AddScoped<IBudgetService, BudgetService>()
            .AddScoped<IBudgetTypeService, BudgetTypeService>()
            .AddScoped<ITransactionService, TransactionService>()
            .AddScoped<ITransactionCategoryService, TransactionCategoryService>()
            .AddScoped<ITransactionTypeService, TransactionTypeService>()
            // CTX: service, do not remove this line
            // ...
            .AddScoped<IMoneyTrackerService, MoneyTrackerService>()
        ;
    }
}