using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeliveryApi.Enums;

namespace DeliveryApi.Models;

public class Order
{
    [Key] 
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    public DateTime OrderTime { get; set; }
    public DateTime DeliveryTime { get; set; }
    public Status Status { get; set; }
    public double Price { get; set; }
    public Guid Address { get; set; }
}