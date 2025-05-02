namespace MoneyTracker.Infrastructure.Repositories;

using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.Entities;

public partial class MoneyTrackerContext : DbContext, IDataProtectionKeyContext
{
    public MoneyTrackerContext()
    {
    }

    public MoneyTrackerContext(DbContextOptions<MoneyTrackerContext> options) : base(options)
    {
    }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
    public virtual DbSet<Example> Examples { get; set; }
    public virtual DbSet<Bank> Banks { get; set; }
    public virtual DbSet<Budget> Budgets { get; set; }
    public virtual DbSet<BudgetType> BudgetTypes { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<TransactionCategory> TransactionCategorys { get; set; }
    public virtual DbSet<TransactionType> TransactionTypes { get; set; }
    // CTX: dbset, do not remove this line

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            throw new InvalidOperationException("Database is not configured");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DataProtectionKey>(entity =>
        {
            entity.ToTable("DataProtectionKeys", "Management");

            entity.Property(e => e.FriendlyName);
            entity.Property(e => e.Xml).IsRequired();
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.ToTable("Bank", "Catalogue");

            entity.HasIndex(e => e.BankName, "UK_BankName").IsUnique();

            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.ToTable("Budget", "Analytics");

            entity.Property(e => e.BudgetAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.BudgetType).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.BudgetTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Budget_BudgetType");

            entity.HasOne(d => d.TransactionCategory).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.TransactionCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Budget_TransactionCategoryId");
        });

        modelBuilder.Entity<BudgetType>(entity =>
        {
            entity.ToTable("BudgetType", "Catalogue");

            entity.HasIndex(e => e.BudgetTypeName, "UK_BudgetTypeName").IsUnique();

            entity.Property(e => e.BudgetTypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transaction", "Analytics");

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionDescription)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Bank).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Bank");

            entity.HasOne(d => d.TransactionCategory).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TransactionCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_TransactionCategory");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TransactionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_TransactionType");
        });

        modelBuilder.Entity<TransactionCategory>(entity =>
        {
            entity.ToTable("TransactionCategory", "Catalogue");

            entity.HasIndex(e => e.TransactionCategoryColor, "UK_TransactionCategoryColor").IsUnique();

            entity.HasIndex(e => e.TransactionCategoryIcon, "UK_TransactionCategoryIcon").IsUnique();

            entity.HasIndex(e => e.TransactionCategoryName, "UK_TransactionCategoryName").IsUnique();

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionCategoryColor)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.TransactionCategoryDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TransactionCategoryIcon)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TransactionCategoryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.ToTable("TransactionType", "Catalogue");

            entity.HasIndex(e => e.TransactionTypeName, "UK_TransactionTypeName").IsUnique();

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionTypeDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });
        // CTX: model builder, do not remove this line
    }
}