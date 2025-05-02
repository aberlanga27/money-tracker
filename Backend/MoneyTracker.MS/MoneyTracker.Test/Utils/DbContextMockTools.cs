namespace MoneyTracker.Test.Utils;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Infrastructure.Repositories;
using Moq;

public static class DbContextMockTools
{
    public static DbContextOptions<MoneyTrackerContext> CreateNewContextOptions()
    {
        var builder = new DbContextOptionsBuilder<MoneyTrackerContext>();
        builder.UseInMemoryDatabase("MoneyTrackerContextTest");
        return builder.Options;
    }

    public static Mock<DbSet<T>> GetMockedDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(default))
            .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestDbAsyncQueryProvider<T>(data.Provider));

        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

        return mockSet;
    }

    public static Mock<MoneyTrackerContext> GetMockedContext()
    {
        var mockContext = new Mock<MoneyTrackerContext>();

        var examples = new List<Example> {
            new() {
                ExampleId = 1,
                // CTX: test-attribute, do not remove this line
            },
            new() {
                ExampleId = 2,
            },
        }.AsQueryable();
        mockContext.Setup(x => x.Examples).Returns(GetMockedDbSet(examples).Object);


        var banks = new List<Bank> {
            new() {
                BankId = 1,
                BankName = "test",
			// CTX: test-attribute, do not remove this line
            },
            new() {
                BankId = 2,
            },
        }.AsQueryable();
        mockContext.Setup(x => x.Banks).Returns(GetMockedDbSet(banks).Object);

        var budgets = new List<Budget> {
            new() {
                BudgetId = 1,
                TransactionCategoryId = 1,
            BudgetTypeId = 1,
            BudgetAmount = 1.0m,
			// CTX: test-attribute, do not remove this line
            },
            new() {
                BudgetId = 2,
            },
        }.AsQueryable();
        mockContext.Setup(x => x.Budgets).Returns(GetMockedDbSet(budgets).Object);

        var budgetTypes = new List<BudgetType> {
            new() {
                BudgetTypeId = 1,
                BudgetTypeName = "test",
            BudgetTypeDays = 1,
			// CTX: test-attribute, do not remove this line
            },
            new() {
                BudgetTypeId = 2,
            },
        }.AsQueryable();
        mockContext.Setup(x => x.BudgetTypes).Returns(GetMockedDbSet(budgetTypes).Object);

        var transactions = new List<Transaction> {
            new() {
                TransactionId = 1,
                TransactionCategoryId = 1,
            TransactionTypeId = 1,
            BankId = 1,
            TransactionAmount = 1.0m,
            TransactionDate = DateTime.Now,
            TransactionDescription = "test",
			// CTX: test-attribute, do not remove this line
            },
            new() {
                TransactionId = 2,
            },
        }.AsQueryable();
        mockContext.Setup(x => x.Transactions).Returns(GetMockedDbSet(transactions).Object);

        var transactionCategorys = new List<TransactionCategory> {
            new() {
                TransactionCategoryId = 1,
                TransactionCategoryName = "test",
            TransactionCategoryDescription = "test",
            TransactionCategoryIcon = "test",
            TransactionCategoryColor = "test",
			// CTX: test-attribute, do not remove this line
            },
            new() {
                TransactionCategoryId = 2,
            },
        }.AsQueryable();
        mockContext.Setup(x => x.TransactionCategorys).Returns(GetMockedDbSet(transactionCategorys).Object);

        var transactionTypes = new List<TransactionType> {
            new() {
                TransactionTypeId = 1,
                TransactionTypeName = "test",
            TransactionTypeDescription = "test",
			// CTX: test-attribute, do not remove this line
            },
            new() {
                TransactionTypeId = 2,
            },
        }.AsQueryable();
        mockContext.Setup(x => x.TransactionTypes).Returns(GetMockedDbSet(transactionTypes).Object);
        // CTX: mock dbset, do not remove this line

        return mockContext;
    }
}