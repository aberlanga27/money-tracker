namespace MoneyTracker.Test.Services;

using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Services;
using Moq;
using Xunit;
using static MoneyTracker.Domain.Entities.Config.MoneyTrackerSettings;

public class PaginatedServiceTest
{
    private readonly PaginatedService paginatedService;
    public PaginatedServiceTest()
    {
        var appSettings = new Mock<MoneyTrackerSettings>();
        appSettings.Object.Pagination = new PaginationModel { DefaultPageSize = 10, MaxPageSize = 100 };

        paginatedService = new PaginatedService(appSettings.Object);
    }

    [Fact]
    public void ValidatePagination_WhenPageSizeIsNull_ReturnsDefaultPageSize()
    {
        int? pageSize = null;
        int? offsetSize = null;
        var totalRecords = 100;

        var (size, offset) = paginatedService.ValidatePagination(pageSize, offsetSize, totalRecords);

        Assert.Equal(10, size);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ValidatePagination_WhenPageSizeIsGreaterThanMaxPageSize_ReturnsMaxPageSize()
    {
        int? pageSize = 200;
        int? offsetSize = null;
        var totalRecords = 100;

        var (size, offset) = paginatedService.ValidatePagination(pageSize, offsetSize, totalRecords);

        Assert.Equal(100, size);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ValidatePagination_WhenPageSizeIsGreaterThanTotalRecords_ReturnsTotalRecords()
    {
        int? pageSize = 200;
        int? offsetSize = null;
        var totalRecords = 50;

        var (size, offset) = paginatedService.ValidatePagination(pageSize, offsetSize, totalRecords);

        Assert.Equal(50, size);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ValidatePagination_WhenPageSizeIsLessThanOne_ReturnsOneAsMinimum()
    {
        int? pageSize = 0;
        int? offsetSize = null;
        var totalRecords = 100;

        var (size, offset) = paginatedService.ValidatePagination(pageSize, offsetSize, totalRecords);

        Assert.Equal(1, size);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ValidatePagination_WhenOffsetSizeIsNull_ReturnsDefaultOffsetSize()
    {
        int? pageSize = 10;
        int? offsetSize = null;
        var totalRecords = 100;

        var (size, offset) = paginatedService.ValidatePagination(pageSize, offsetSize, totalRecords);

        Assert.Equal(10, size);
        Assert.Equal(0, offset);
    }
}