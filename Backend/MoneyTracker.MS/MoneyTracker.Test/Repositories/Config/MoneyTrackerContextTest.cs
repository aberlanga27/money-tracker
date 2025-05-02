namespace MoneyTracker.Test.Repositories;

using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using MoneyTracker.Infrastructure.Repositories;
using MoneyTracker.Test.Utils;
using Xunit;

public class MoneyTrackerContextTest
{
    [Fact]
    public void OnConfiguring_WhenNotConfigured()
    {
        var context = new MoneyTrackerContext();

        var exception = Assert.Throws<InvalidOperationException>(() => context.Database.EnsureCreated());
        Assert.Equal("Database is not configured", exception.Message);
    }

    [Fact]
    public void OnModelCreating_ConfigureData()
    {
        var options = DbContextMockTools.CreateNewContextOptions();
        using var context = new MoneyTrackerContext(options);

        var model = context.Model;
        var entity = model.FindEntityType(typeof(DataProtectionKey));
        Assert.NotNull(entity);
    }
}