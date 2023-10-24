using System.ComponentModel.DataAnnotations;

namespace DeliveryApi.Models;

public class BasketDTO
{
    [Key]
    public Guid DishId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    [Required]
    public double TotalPrice { get; set; }
    
    public string? Image { get; set; }
}