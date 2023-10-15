using DeliveryApi.Enums;
using DeliveryApi.Models;

namespace DeliveryApi.Services;

public interface IDishService
{
    public Task<DishesMenuResponse> GetDishMenu(DishCategory? category, bool vegeterian, DishSorting sortingBy, int page);

    public Task AddDishes(List<DishDTO> model);
}