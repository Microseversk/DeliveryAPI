using DeliveryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Context;

public class DeliveryContext : DbContext
{
    public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options)
    {
    }

    public DbSet<UserDTO> Users { get; set; }
    public DbSet<Dish> Dish { get; set; }
    public DbSet<Rating> Rating { get; set; }
    public DbSet<DishingCart> DishingCart { get; set; }
}