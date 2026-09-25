using System;
using System.Collections.Generic;
using BankLib.AccountSystem;
using BankLib.Login;
using Microsoft.EntityFrameworkCore;

namespace CollectionsTask.DB;

public partial class SampleDbContext : DbContext
{
    public DbSet<LoginAccount> Logins {get; set;}
    public DbSet<Account> Accounts {get; set;}
    public DbSet<CheckingAccount> CheckingAccounts {get; set;}
    public DbSet<SavingsAccount> SavingsAccounts {get; set;}
    public DbSet<LoanAccount> LoanAccounts {get; set;}
    public DbSet<BankLib.Transactions.Transaction> Transactions {get; set;}
    public DbSet<BankLib.Transactions.TransferTransaction> TransferTransactions {get; set;}
    public DbSet<Checkbook> Checkbooks {get; set;}
    public SampleDbContext()
    {
    }

    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define the Login Table
        modelBuilder.Entity<LoginAccount>( entity =>
        {
            // Creates a Primary Key
            entity.HasKey(login => login.ID);

            entity.Property(login => login.ID)
                .ValueGeneratedOnAdd();

            entity.Property(login => login.FirstName)
                .IsRequired()
                .HasMaxLength(32);
            
            entity.Property(login => login.LastName)
                .IsRequired()
                .HasMaxLength(32);
            
            entity.Property(login => login.Username)
                .IsRequired()
                .HasMaxLength(64);

            entity.Property(login => login.Password)
                .IsRequired()
                .HasMaxLength(64);

            entity.HasIndex(login => login.Username)
                .IsUnique();
            
            entity.Property(login => login.IsAdmin)
                .IsRequired()
                .HasDefaultValue(false);
        });

        // Defite the Account Table
        modelBuilder.Entity<Account>( entity =>
        {
            // Primary Key Creation
            entity.HasKey(account => account.AccountNumber);
            entity.ToTable("Accounts");

            entity.HasDiscriminator<string>("AccountKind")
                .HasValue<CheckingAccount>(Account.Type.Checking.ToString())
                .HasValue<SavingsAccount>(Account.Type.Savings.ToString())
                .HasValue<LoanAccount>(Account.Type.Loan.ToString());

            // Foreign Key Relationship Creation
            // (Login -Many-> Accounts)
            // (Account -One-> Login)
            entity.HasOne<LoginAccount>()
                .WithMany()
                .HasForeignKey(account => account.AssociatedID)
                .HasPrincipalKey(login => login.ID)
                .OnDelete(DeleteBehavior.Cascade);

            // Account Number already auto-generates
            entity.Property(account => account.AccountNumber)
                .IsRequired();

            // Account already gets the first name and last name from it's 
            // Associated Login Acocunt Owner so it might be redundant to store it
            entity.Property(account => account.AccountType)
                .IsRequired();

            // Balance is required and should be stored as a decimal with 2 decimal places
            entity.Property(account => account.Balance)
                .IsRequired()
                .HasColumnType("Decimal(19,2)");
            
            entity.Property(account => account.IsActive)
                .IsRequired();
        });

        modelBuilder.Entity<CheckingAccount>( entity =>
        {
            entity.Property(account => account.IsOverdraftEnabled)
                  .IsRequired()
                  .HasDefaultValue(false);
                  
            entity.Property(account => account.OverdraftLimit)
                  .IsRequired()
                  .HasColumnType("Decimal(19,2)")
                  .HasDefaultValue(0.00m);
        });
        modelBuilder.Entity<SavingsAccount>();
        modelBuilder.Entity<LoanAccount>();

        // Define Transaction Table
        modelBuilder.Entity<BankLib.Transactions.Transaction>( entity =>
        {
            entity.HasKey(transaction => transaction.ID);

            entity.Property(transaction => transaction.ID)
                .ValueGeneratedOnAdd();

            entity.Property(transaction => transaction.AccountNumber)
                .IsRequired();

            entity.Property(transaction => transaction.Amount)
                .IsRequired()
                .HasColumnType("Decimal(19,2)");

            entity.Property(transaction => transaction.Date)
                .IsRequired();
        });

        modelBuilder.Entity<BankLib.Transactions.TransferTransaction>( entity =>
        {
            entity.Property(transaction => transaction.ToAccountNumber)
                .IsRequired();
        });

        modelBuilder.Entity<BankLib.AccountSystem.Checkbook>(entity =>
        {
            entity.HasKey(checkbook => new
            {
                checkbook.AccountNumber,
                checkbook.ID
            });

            entity.HasOne<Account>()
                .WithMany()
                .HasForeignKey(account => account.AccountNumber)
                .HasPrincipalKey(checkbook => checkbook.AccountNumber)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(checkbook => checkbook.ID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            entity.Property(checkbook => checkbook.AccountNumber)
                .IsRequired();

            entity.Property(checkbook => checkbook.Active)
                .IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
