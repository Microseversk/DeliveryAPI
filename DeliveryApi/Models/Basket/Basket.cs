using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApi.Models;

public class Basket
{
    [Key]
    public Guid UserId { get; set; }
    public Guid DishId { get; set; }
    public int Amount { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("DishId")]
    public Dish Dish { get; set; }

}