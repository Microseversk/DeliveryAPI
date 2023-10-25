using DeliveryApi.Enums;
using DeliveryApi.Models;

namespace DeliveryApi.Services;

public interface IDishService
{
    public Task<DishesMenuResponse> GetDishMenu(DishCategory? category, bool vegeterian, DishSorting sortingBy,
        int page);

    public Task AddDishes(List<Dish> model);

    public Task<Dish> GetDishById(Guid id);

    public Task<Rating> CheckUserRated(string token, Guid dishId);
    public Task PutUserRating(string token, Guid dishId, double value);
}