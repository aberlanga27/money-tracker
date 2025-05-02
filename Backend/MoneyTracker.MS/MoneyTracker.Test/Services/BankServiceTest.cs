namespace MoneyTracker.Test.Services;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Services;
using Moq;
using Xunit;
using static MoneyTracker.Domain.Entities.Config.MoneyTrackerSettings;

public class BankServiceTest
{
    private readonly Mock<ICacheProvider> cacheProvider = new();
    private readonly Mock<IBankRepository> bankRepository = new();
    private readonly BankService bankService;
    private readonly ValueResponse<Bank> okDefaultResponse = new()
    {
        Status = true,
        Message = "Ok",
        Response = new Bank { }
    };
    private readonly ValueResponse<Bank> errorDefaultResponse = new()
    {
        Status = false,
        Message = "Error"
    };

    public BankServiceTest()
    {
        var appSettings = new Mock<MoneyTrackerSettings>();
        appSettings.Object.Pagination = new PaginationModel { DefaultPageSize = 10, MaxPageSize = 100 };

        bankService = new BankService(
            appSettings.Object,
            new Mock<ILocalizationProvider>().Object,
            cacheProvider.Object,
            bankRepository.Object
        );
    }

    [Fact]
    public void GetAllBanks()
    {
        bankRepository.Setup(x => x.GetAllBanks(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = bankService.GetAllBanks();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BankDTO>>(result);
    }

    [Fact]
    public async Task GetAllBanks_WithPagination()
    {
        bankRepository.Setup(x => x.GetAllBanks(It.IsAny<int>(), It.IsAny<int>()))
            .Returns([]);

        var result = await bankService.GetAllBanks(null, null);

        Assert.NotNull(result);
        Assert.IsType<PaginationResponse<IEnumerable<BankDTO>>>(result);
    }

    [Fact]
    public async Task GetBankById()
    {
        bankRepository.Setup(x => x.GetBankById(It.IsAny<int>()))
            .ReturnsAsync(new Bank());

        var result = await bankService.GetBankById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BankDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetBankById_Cache()
    {
        cacheProvider.Setup(x => x.GetAsync<BankDTO>(It.IsAny<string>()))
            .ReturnsAsync(new BankDTO());

        var result = await bankService.GetBankById(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BankDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task GetBankById_InvalidId()
    {
        bankRepository.Setup(x => x.GetBankById(It.IsAny<int>()))
            .ReturnsAsync((Bank?)null);

        var result = await bankService.GetBankById(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BankDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateBank()
    {
        bankRepository.Setup(x => x.CreateBank(It.IsAny<Bank>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await bankService.CreateBank(new BankDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BankDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task CreateBank_InvalidData()
    {
        bankRepository.Setup(x => x.CreateBank(It.IsAny<Bank>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await bankService.CreateBank(new BankDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BankDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateBank()
    {
        bankRepository.Setup(x => x.UpdateBank(It.IsAny<Bank>()))
            .ReturnsAsync(okDefaultResponse);

        var result = await bankService.UpdateBank(new BankDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BankDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task UpdateBank_InvalidData()
    {
        bankRepository.Setup(x => x.UpdateBank(It.IsAny<Bank>()))
            .ReturnsAsync(errorDefaultResponse);

        var result = await bankService.UpdateBank(new BankDTO { });

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BankDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteBank()
    {
        bankRepository.Setup(x => x.DeleteBank(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<Bank> { Status = true, Response = new Bank { } });

        var result = await bankService.DeleteBank(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BankDTO>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteBank_InvalidId()
    {
        bankRepository.Setup(x => x.DeleteBank(It.IsAny<int>()))
            .ReturnsAsync(new ValueResponse<Bank> { Status = false });

        var result = await bankService.DeleteBank(-1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<BankDTO>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchBanks()
    {
        bankRepository.Setup(x => x.SearchBanks(It.IsAny<string>()))
            .Returns([]);

        var result = bankService.SearchBanks("search");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BankDTO>>(result);
    }

    [Fact]
    public void GetBanksByAttributes()
    {
        bankRepository.Setup(x => x.GetBanksByAttributes(It.IsAny<Bank>()))
            .Returns([]);

        var result = bankService.GetBanksByAttributes(new BankDTO { });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BankDTO>>(result);
    }
}