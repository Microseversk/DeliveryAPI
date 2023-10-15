namespace DeliveryApi.Models;

public class DishesMenuResponse
{
    public List<DishDTO> Dishes { get; set; }
    public PageInfo Page { get; set; }
}