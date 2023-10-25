using DeliveryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Context;

public class DeliveryContext : DbContext
{
    public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasAlternateKey(u => u.Email);
        modelBuilder.Entity<OrderDishes>().HasKey(od => new { od.OrderId, od.DishId });
        modelBuilder.Entity<Rating>().HasKey(r => new { r.UserId, r.DishId });
        modelBuilder.Entity<Basket>().HasKey(b => new { b.UserId, b.DishId });
    }

    public DbSet<User> User { get; set; }
    public DbSet<Dish> Dish { get; set; }
    public DbSet<Rating> Rating { get; set; }
    public DbSet<Basket> Basket { get; set; }
    public DbSet<Order> Order { get; set; }
    
    public DbSet<OrderDishes> OrderDishes { get; set; }
    public DbSet<BannedToken> BannedTokens { get; set; }
}