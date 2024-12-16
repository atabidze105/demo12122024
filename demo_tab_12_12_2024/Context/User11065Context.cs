using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using demo_tab_12_12_2024.Models;

namespace demo_tab_12_12_2024.Context;

public partial class User11065Context : DbContext
{
    public User11065Context()
    {
    }

    public User11065Context(DbContextOptions<User11065Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductGallery> ProductGalleries { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=192.168.7.159:5432;Database=user11048;Username=user11048;Password=73248");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manufacturer_pk");

            entity.ToTable("manufacturer", "demo1212_clone");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.PatronageStart).HasColumnName("patronage_start");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pk");

            entity.ToTable("product", "demo1212_clone");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Manufacturer).HasColumnName("manufacturer");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.ManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Manufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_manufacturer_fk");

            entity.HasMany(d => d.AttachedProduct1s).WithMany(p => p.MainProducts)
                .UsingEntity<Dictionary<string, object>>(
                    "AttachedProduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("AttachedProduct1")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("attached_products_product_fk_1"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("MainProduct")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("attached_products_product_fk"),
                    j =>
                    {
                        j.HasKey("MainProduct", "AttachedProduct1").HasName("attached_products_pk");
                        j.ToTable("attached_products", "demo1212_clone");
                        j.IndexerProperty<int>("MainProduct").HasColumnName("main_product");
                        j.IndexerProperty<int>("AttachedProduct1").HasColumnName("attached_product");
                    });

            entity.HasMany(d => d.MainProducts).WithMany(p => p.AttachedProduct1s)
                .UsingEntity<Dictionary<string, object>>(
                    "AttachedProduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("MainProduct")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("attached_products_product_fk"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("AttachedProduct1")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("attached_products_product_fk_1"),
                    j =>
                    {
                        j.HasKey("MainProduct", "AttachedProduct1").HasName("attached_products_pk");
                        j.ToTable("attached_products", "demo1212_clone");
                        j.IndexerProperty<int>("MainProduct").HasColumnName("main_product");
                        j.IndexerProperty<int>("AttachedProduct1").HasColumnName("attached_product");
                    });
        });

        modelBuilder.Entity<ProductGallery>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("product_gallery", "demo1212_clone");

            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_gallery_product_fk");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sales_pk");

            entity.ToTable("sales", "demo1212_clone");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Product).HasColumnName("product");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SaleDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sale_date");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sales_product_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
