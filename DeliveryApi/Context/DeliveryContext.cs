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
        modelBuilder.Entity<Rating>().HasKey(r => new { r.UserId, r.DishId });
        modelBuilder.Entity<UserDTO>().HasAlternateKey(u =>u.Email);
    }

    public DbSet<UserDTO> User { get; set; }
    public DbSet<DishDTO> Dish { get; set; }
    public DbSet<Rating> Rating { get; set; }
    public DbSet<DishingCart> DishingCart { get; set; }
}