using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HALLMiniKPay.Database.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblWallet> TblWallets { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            string connectionString = "Data Source=DESKTOP-UST9CM1\\SQLEXPRESS;Initial Catalog=Banking;User ID=sa;Password=sasa@123;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblWallet>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("Tbl_Wallet");

            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Pin).HasMaxLength(10);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
