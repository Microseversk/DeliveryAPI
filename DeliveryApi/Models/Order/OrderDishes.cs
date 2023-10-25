using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApi.Models;

public class OrderDishes
{
    [Key]
    public Guid OrderId { get; set; }
    public Guid DishId { get; set; }
    
    public int Amount { get; set; }
    
    [ForeignKey("OrderId")]
    public Order Order {get; set; }
    
    [ForeignKey("DishId")]
    public Dish Dish {get; set; }
}