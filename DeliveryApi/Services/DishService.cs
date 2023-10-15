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

    public async Task<DishesMenuResponse> GetDishMenu(DishCategory? category, bool vegeterian, DishSorting sortingBy,
        int page)
    {
        double DISHES_ON_PAGE = 3;
        PageInfo pageInfo = new PageInfo { PageSize = (int)DISHES_ON_PAGE, CurrentPage = page};
        IQueryable<DishDTO> reqDishes;

        if (category != null)
        {
            reqDishes = _context.Dish
                .Where(dish => dish.DishCategory == category)
                .Where(dish =>
                    (vegeterian == false)
                        ? dish.IsVegetarian == true || dish.IsVegetarian == false
                        : dish.IsVegetarian == true);
            
        }
        else
        {
            reqDishes = _context.Dish.Where(dish =>
                (vegeterian == false)
                    ? dish.IsVegetarian == true || dish.IsVegetarian == false
                    : dish.IsVegetarian == true);
        }
        
        pageInfo.PageCount = (int)Math.Ceiling(reqDishes.ToList().Count() / DISHES_ON_PAGE);

        switch (sortingBy)
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

        var gottedDishes = reqDishes.ToList();
        var showedDishes = gottedDishes.Skip((pageInfo.CurrentPage - 1) * pageInfo.PageSize).Take(pageInfo.PageSize).ToList();

        if (showedDishes.Count == 0)
        {
            throw new Exception(message: "Invalid page value");
        }
        
        return new DishesMenuResponse { Dishes = showedDishes, Page = pageInfo };
    }

    
    public async Task AddDishes(List<DishDTO> model)
    {
        await _context.Dish.AddRangeAsync(model);
        await _context.SaveChangesAsync();
    }
}