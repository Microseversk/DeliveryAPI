using DeliveryApi.Enums;

namespace DeliveryApi.Models;

public class Dish
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int Price { get; set; }
    
    public Category Category { get; set; }
    
    public string Description { get; set; }
    
    public bool IsVegetarian { get; set; }
    
    public string Photo { get; set; }
}