namespace MoneyTracker.Test.Repositories;

using MoneyTracker.Domain.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Infrastructure.Repositories;
using MoneyTracker.Test.Utils;
using Moq;
using Xunit;

public class BankRepositoryTest
{
    private readonly Mock<MoneyTrackerContext> mockContext;
    private readonly BankRepository bankRepository;

    public BankRepositoryTest()
    {
        mockContext = DbContextMockTools.GetMockedContext();
        bankRepository = new BankRepository(mockContext.Object, new Mock<ILocalizationProvider>().Object);
    }

    [Fact]
    public async Task GetCount()
    {
        var result = await bankRepository.GetCount();

        Assert.IsType<int>(result);
    }

    [Fact]
    public void GetAllBanks()
    {
        var result = bankRepository.GetAllBanks();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Bank>>(result);
    }

    [Fact]
    public void GetAllBanks_WithPagination()
    {
        var result = bankRepository.GetAllBanks(0, 0);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Bank>>(result);
    }

    [Fact]
    public async Task GetBankById()
    {
        var result = await bankRepository.GetBankById(1);

        Assert.NotNull(result);
        Assert.IsType<Bank>(result);
    }

    [Fact]
    public async Task CreateBank()
    {
        var bank = new Bank()
        {
            BankId = 0,
            BankName = "test_new",
            // CTX: new-test-attribute, do not remove this line
        };

        var result = await bankRepository.CreateBank(bank);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.True(result.Status);
        Assert.Equal(bank, result.Response);
    }

    [Fact]
    public async Task CreateBank_InvalidUK()
    {
        var bank = new Bank()
        {
            BankId = 0,
            BankName = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await bankRepository.CreateBank(bank);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateBank_InvalidFK()
    {
        var bank = new Bank()
        {
            BankId = 0,
            BankName = null!,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await bankRepository.CreateBank(bank);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task CreateBank_Update()
    {
        var bank = new Bank()
        {
            BankId = 1,
            BankName = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await bankRepository.CreateBank(bank);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.True(result.Status);
        Assert.Equal(bank, result.Response);
    }

    [Fact]
    public async Task UpdateBank()
    {
        var bank = new Bank()
        {
            BankId = 1,
            BankName = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await bankRepository.UpdateBank(bank);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.True(result.Status);
        Assert.Equal(bank, result.Response);
    }

    [Fact]
    public async Task UpdateBank_InvalidUK()
    {
        var bank = new Bank()
        {
            BankId = 2,
            BankName = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await bankRepository.UpdateBank(bank);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateBank_InvalidFK()
    {
        var bank = new Bank()
        {
            BankId = 2,
            BankName = null!,
            // CTX: invalid-test-attribute, do not remove this line
        };

        var result = await bankRepository.UpdateBank(bank);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task UpdateBank_NotFound()
    {
        var bank = new Bank()
        {
            BankId = 3,
            BankName = "test",
            // CTX: test-attribute, do not remove this line
        };

        var result = await bankRepository.UpdateBank(bank);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteBank()
    {
        var result = await bankRepository.DeleteBank(2);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.True(result.Status);
    }

    [Fact]
    public async Task DeleteBank_InvalidFK()
    {
        var result = await bankRepository.DeleteBank(1);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task DeleteBank_NotFound()
    {
        var result = await bankRepository.DeleteBank(3);

        Assert.NotNull(result);
        Assert.IsType<ValueResponse<Bank>>(result);
        Assert.False(result.Status);
    }

    [Fact]
    public void SearchBanks()
    {
        var result = bankRepository.SearchBanks("1");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Bank>>(result);
    }

    [Fact]
    public void GetBanksByAttributes()
    {
        var result = bankRepository.GetBanksByAttributes(new Bank
        {
            BankId = 1,
            BankName = "test",
            // CTX: test-attribute, do not remove this line
        });

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Bank>>(result);
    }
}