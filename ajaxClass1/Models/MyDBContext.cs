﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ajaxClass1.Models;

public partial class MyDBContext : DbContext
{
    public MyDBContext(DbContextOptions<MyDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Spot> Spots { get; set; }

    public virtual DbSet<SpotImage> SpotImages { get; set; }

    public virtual DbSet<SpotImagesSpot> SpotImagesSpots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City)
                .HasMaxLength(10)
                .HasColumnName("city");
            entity.Property(e => e.Road)
                .HasMaxLength(200)
                .HasColumnName("road");
            entity.Property(e => e.SiteId)
                .HasMaxLength(50)
                .HasColumnName("site_id");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FileName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Salt).HasMaxLength(200);
        });

        modelBuilder.Entity<Spot>(entity =>
        {
            entity.Property(e => e.SpotId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasMaxLength(20);
            entity.Property(e => e.Longitude).HasMaxLength(20);
            entity.Property(e => e.SpotTitle).HasMaxLength(50);
        });

        modelBuilder.Entity<SpotImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);
        });

        modelBuilder.Entity<SpotImagesSpot>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SpotImagesSpot");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasMaxLength(20);
            entity.Property(e => e.Longitude).HasMaxLength(20);
            entity.Property(e => e.SpotTitle).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}