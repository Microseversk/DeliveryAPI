using System.ComponentModel.DataAnnotations;
using DeliveryApi.Enums;

namespace DeliveryApi.Models;

public class Dish
{
    [Key]
    public Guid Id { get; set; }
    
    [MinLength(1)]
    public string Name { get; set; }
    
    public int Price { get; set; }
    
    public DishCategory Category { get; set; }
    
    public string? Description { get; set; }
    
    public bool Vegetarian { get; set; }
    
    public string? Image { get; set; }
    
    public double? Rating { get; set; }
}