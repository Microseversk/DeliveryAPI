using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApi.Models;

public class DishingCart
{
    [Key]
    public Guid UserId { get; set; }
    
    public Guid DishId { get; set; }
    
    [ForeignKey("UserId")]
    public UserDTO User { get; set; }
    
    [ForeignKey("DishId")]
    public DishDTO Dish { get; set; }
}