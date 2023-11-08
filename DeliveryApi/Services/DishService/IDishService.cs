using DeliveryApi.Enums;
using DeliveryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Services;

public interface IDishService
{
    public Task<DishesMenuResponse> GetDishMenu(List<DishCategory>category, bool vegeterian, DishSorting sortingBy,
        int page);

    public Task<Dish> GetDishById(Guid id);

    public Task<Rating> CheckUserRated(string token, Guid dishId);
    public Task PutUserRating(string token, Guid dishId, double value);
}