using System.ComponentModel.DataAnnotations;
using DeliveryApi.Enums;
using DeliveryApi.Migrations;

namespace DeliveryApi.Models;

public class OrderDTO
{
    [Key] public Guid Id { get; set; }
    public DateTime DeliveryTime { get; set; }
    public DateTime OrderTime { get; set; }
    public Status Status { get; set; }
    public double Price { get; set; }
    public List<BasketDTO> Dishes { get; set; }
    
    public string Address { get; set; }
}