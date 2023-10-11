using DeliveryApi.Enums;

namespace DeliveryApi.Models;

public class Order
{
    public int Id { get; set; }
    public DishingCart DishingCart { get; set; }
    public Status Status { get; set; }
    public int Price { get; set; }
    public Guid AddressId { get; set; }
    public DateTime DeliveryTime { get; set; }
    public DateTime OrderTime { get; set; }
}