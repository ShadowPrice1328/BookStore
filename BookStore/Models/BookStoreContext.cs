using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models;

public partial class BookStoreContext : DbContext
{
    public BookStoreContext()
    {
    }

    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderBook> OrderBooks { get; set; }

    public virtual DbSet<OrderShipment> OrderShipments { get; set; }

    public virtual DbSet<Recipient> Recipients { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBook> UserBooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:BookStore");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC07105E7BDE");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.IdUser, "UQ__Admin__B7C9263958E7AD94").IsUnique();

            entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Admin__IdUser__693CA210");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__3214EC07D8AE4119");

            entity.ToTable("Book");

            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.Language)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OriginalName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PublishingHouse)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Series)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Translator)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC073A19E25B");

            entity.ToTable("Order");

            entity.Property(e => e.City)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasColumnType("text");

            entity.HasOne(d => d.Recipient).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RecipientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__Recipient__4D94879B");
        });

        modelBuilder.Entity<OrderBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderBoo__3214EC0763164BAF");

            entity.ToTable("OrderBook");

            entity.HasOne(d => d.IdBookNavigation).WithMany(p => p.OrderBooks)
                .HasForeignKey(d => d.IdBook)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderBook__IdBoo__5165187F");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderBooks)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderBook__IdOrd__5070F446");
        });

        modelBuilder.Entity<OrderShipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderShi__3214EC073AD92B6A");

            entity.ToTable("OrderShipment");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderShipments)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderShip__IdOrd__6477ECF3");

            entity.HasOne(d => d.IdShipmentNavigation).WithMany(p => p.OrderShipments)
                .HasForeignKey(d => d.IdShipment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderShip__IdShi__656C112C");
        });

        modelBuilder.Entity<Recipient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipien__3214EC0747FEA931");

            entity.ToTable("Recipient");

            entity.Property(e => e.FirstName)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shipment__3214EC076608F2CF");

            entity.ToTable("Shipment");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC070C529AC9");

            entity.ToTable("User");

            entity.Property(e => e.FirstName)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Mail).HasMaxLength(255);
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserBook__3214EC07F648E868");

            entity.ToTable("UserBook");

            entity.HasOne(d => d.IdBookNavigation).WithMany(p => p.UserBooks)
                .HasForeignKey(d => d.IdBook)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserBook__IdBook__5812160E");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserBooks)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserBook__IdUser__571DF1D5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
