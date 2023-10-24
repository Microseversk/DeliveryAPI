using DeliveryApi.Validators;

namespace DeliveryApi.Models;

public class OrderCreateDTO
{
    public DateTime DeliveryTime { get; set; }
    public Guid AddressId { get; set; }
}