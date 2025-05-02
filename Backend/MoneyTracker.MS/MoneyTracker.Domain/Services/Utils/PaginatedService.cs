namespace MoneyTracker.Domain.Services;

using Ardalis.GuardClauses;
using MoneyTracker.Domain.Entities.Config;

public class PaginatedService(
    MoneyTrackerSettings appSettings
)
{
    private readonly MoneyTrackerSettings appSettings = Guard.Against.Null(appSettings);

    public (int size, int offset) ValidatePagination(int? pageSize, int? offsetSize, int totalRecords)
    {
        var pagination = appSettings.Pagination;

        if (totalRecords <= 0)
            return (0, 0);

        var validatedPageSize = Math.Clamp(
            pageSize ?? pagination.DefaultPageSize,
            1,
            Math.Min(pagination.MaxPageSize, totalRecords)
        );

        var validatedOffsetSize = Math.Max(offsetSize ?? 0, 0);

        validatedPageSize = Math.Min(validatedPageSize, totalRecords - validatedOffsetSize);

        return (validatedPageSize, validatedOffsetSize);
    }
}