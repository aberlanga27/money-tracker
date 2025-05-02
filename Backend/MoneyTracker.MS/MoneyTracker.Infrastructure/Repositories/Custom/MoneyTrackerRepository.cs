namespace MoneyTracker.Infrastructure.Repositories;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;

[ExcludeFromCodeCoverage]
public class MoneyTrackerRepository(
    MoneyTrackerContext context
) : Repository<MoneyTracker>(context), IMoneyTrackerRepository
{
    public virtual async Task<DateTime?> HealthCheckup()
    {
        DateTime? serverDateTime = await context.Database.SqlQuery<DateTime>($"SELECT GETDATE() AS Value").FirstOrDefaultAsync();
        return serverDateTime;
    }
}