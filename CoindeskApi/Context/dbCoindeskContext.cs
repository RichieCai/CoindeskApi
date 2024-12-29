using System;
using System.Collections.Generic;
using CoindeskApi.Models.MetaData;
using Microsoft.EntityFrameworkCore;

namespace CoindeskApi.Context;

public partial class dbCoindeskContext : DbContext
{
    public dbCoindeskContext(DbContextOptions<dbCoindeskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coindesk> Coindesks { get; set; }

    public virtual DbSet<CoindeskTw> CoindeskTws { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coindesk>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Coindesk");

            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CodeName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.RateFloat).HasColumnType("decimal(9, 4)");
            entity.Property(e => e.Symbol)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasPrecision(0);
        });

        modelBuilder.Entity<CoindeskTw>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CoindeskTW");

            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CodeName).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
