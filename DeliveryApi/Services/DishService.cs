using System.Diagnostics;
using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Models;

namespace DeliveryApi.Services;

public class DishService : IDishService
{
    private readonly DeliveryContext _context;

    public DishService(DeliveryContext context)
    {
        _context = context;
    }
    public async Task<(List<DishDTO>, PageInfo)> GetDishMenu(DishCategory category, bool vegeterian, DishSorting sortingBy, int page)
    {
        var reqDishes = _context.Dish.Where(dish => dish.DishCategory == category && dish.IsVegetarian == vegeterian);
        
        switch(sortingBy)
        {
            case (DishSorting.NameAsc):
            {
                reqDishes = reqDishes.OrderBy(d => d.Name);
                break;
            }
            case (DishSorting.NameDesc):
            {
                reqDishes = reqDishes.OrderByDescending(d => d.Name);
                break;
            }
            case (DishSorting.PriceAsc):
            {
                reqDishes = reqDishes.OrderBy(d => d.Price);
                break;
            }
            case (DishSorting.PriceDesc):
            {
                reqDishes = reqDishes.OrderByDescending(d => d.Price);
                break;
            }
            case (DishSorting.RatingAsc):
            {
                reqDishes = reqDishes.OrderBy(d => d.Rating);
                break;
            }
            case (DishSorting.RatingDesc):
            {
                reqDishes = reqDishes.OrderByDescending(d => d.Rating);
                break;
            }
            default:
                break;
        }

        var menu = (reqDishes.ToList(),new PageInfo{CurrentPage = page});
        return menu;
    }

    public async Task AddDishes(List<DishDTO> model)
    {
        await _context.Dish.AddRangeAsync(model);
        await _context.SaveChangesAsync();
    }
}