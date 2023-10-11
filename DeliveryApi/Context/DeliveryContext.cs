using DeliveryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Context;

public class DeliveryContext : DbContext
{
    public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options)
    {
    }

    public DbSet<User> _Users { get; set; }
    public DbSet<Dish> _Dish { get; set; }
    public DbSet<Rating> _Rating { get; set; }
    public DbSet<DishingCart> _DishingCart { get; set; }
}