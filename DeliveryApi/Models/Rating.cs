using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApi.Models;

public class Rating
{
    [Range(0, 10, ErrorMessage = "Value must be between 0 and 10")]
    public int Value { get; set; }
    
    [Key,ForeignKey("Dish")]
    public Guid DishId { get; set; }
    
    public UserDTO UserDto { get; set; }
}