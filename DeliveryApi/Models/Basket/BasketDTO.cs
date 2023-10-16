namespace DeliveryApi.Models;

public class BasketDTO
{
    public Guid DishId { get; set; }
    
    public string Name { get; set; }
    
    public double Price { get; set; }
    
    public int Amount { get; set; }
    
    public double TotalPrice { get; set; }
    
    public string Image { get; set; }
}