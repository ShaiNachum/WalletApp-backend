using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using WalletApp.DataLayer.Configuration;

namespace WalletApp.DataLayer.Models;

public partial class WalletDbContext : DbContext
{
    private readonly DatabaseConfiguration _configuration;

    public WalletDbContext(DatabaseConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public WalletDbContext(DbContextOptions<WalletDbContext> options)
            : base(options)
    {
        _configuration = null!; // This constructor is for DI scenarios
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Balance> Balances { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-P7TJ07G\\SQLEXPRESS;Database=WALLET;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA586B65FE086");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountName).HasMaxLength(100);
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

            entity.HasOne(d => d.Owner).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Accounts__OwnerI__4D94879B");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E8F3E0E45A");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.AdminName).HasMaxLength(100);
            entity.Property(e => e.AdminPassword).HasMaxLength(100);
            entity.Property(e => e.AdminTaz).HasMaxLength(50);
        });

        modelBuilder.Entity<Balance>(entity =>
        {
            entity.HasKey(e => e.BalanceId).HasName("PK__Balances__A760D59EF788287E");

            entity.Property(e => e.BalanceId).HasColumnName("BalanceID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.BalanceTime).HasColumnType("datetime");
            entity.Property(e => e.BalanceValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

            entity.HasOne(d => d.Account).WithMany(p => p.Balances)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Balances__Accoun__5070F446");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PK__Owners__8193859886095956");

            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.OwnerName).HasMaxLength(100);
            entity.Property(e => e.OwnerPassword).HasMaxLength(100);
            entity.Property(e => e.OwnerTaz).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4BEA60811C");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.AccountGetId).HasColumnName("AccountGetID");
            entity.Property(e => e.AccountPayId).HasColumnName("AccountPayID");
            entity.Property(e => e.TransactionAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionTime).HasColumnType("datetime");

            entity.HasOne(d => d.AccountGet).WithMany(p => p.TransactionAccountGets)
                .HasForeignKey(d => d.AccountGetId)
                .HasConstraintName("FK__Transacti__Accou__5441852A");

            entity.HasOne(d => d.AccountPay).WithMany(p => p.TransactionAccountPays)
                .HasForeignKey(d => d.AccountPayId)
                .HasConstraintName("FK__Transacti__Accou__534D60F1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
