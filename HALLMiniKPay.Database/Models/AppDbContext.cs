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

    public virtual DbSet<TblTransition> TblTransitions { get; set; }

    public virtual DbSet<TblWallet> TblWallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-UST9CM1\\SQLEXPRESS;Database=Banking;User Id=sa;Password=sasa@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblTransition>(entity =>
        {
            entity.HasKey(e => e.TransitionId);

            entity.ToTable("Tbl_Transition");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.FromPhone).HasMaxLength(10);
            entity.Property(e => e.ToPhone).HasMaxLength(10);
        });

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
