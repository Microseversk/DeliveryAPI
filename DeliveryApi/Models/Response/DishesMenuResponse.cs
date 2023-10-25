namespace DeliveryApi.Models;

public class DishesMenuResponse
{
    public List<Dish> Dishes { get; set; }
    public PageInfo Page { get; set; }
}