﻿using Microsoft.EntityFrameworkCore;
using Ecommerce.Orders.Api.Entities;

namespace Ecommerce.Orders.Api.Data;
public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {

    }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderProduct>().HasKey(op => new {op.OrderId, op.ProductId});

        modelBuilder.Entity<OrderProduct>()
        .HasOne(op => op.Order)
        .WithMany(o => o.OrderProducts)
        .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId);

        modelBuilder.Entity<Product>()
        .Property(p => p.Price)
        .HasPrecision(18, 2);
    }
}
