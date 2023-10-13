using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApi.Models;

public class DishingCart
{
    [Key,ForeignKey("User")]
    public string UserEmail { get; set; }
    
    public List<Dish>? Dishes { get; set; }
}