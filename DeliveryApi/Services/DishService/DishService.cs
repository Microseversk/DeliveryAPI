using System.Diagnostics;
using System.Security.Claims;
using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Services;

public class DishService : IDishService
{
    private readonly DeliveryContext _context;
    private readonly IConfiguration _config;

    public DishService(DeliveryContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<DishesMenuResponse> GetDishMenu(DishCategory? category, bool vegeterian, DishSorting sortingBy,
        int page)
    {
        double DISHES_ON_PAGE = double.Parse(_config["Page:DishesOnPage"]);
        PageInfo pageInfo = new PageInfo { PageSize = (int)DISHES_ON_PAGE, CurrentPage = page };
        IQueryable<DishDTO> reqDishes;

        if (category != null)
        {
            reqDishes = _context.Dish
                .Where(dish => dish.Category == category)
                .Where(dish =>
                    (vegeterian == false)
                        ? dish.Vegetarian == true || dish.Vegetarian == false
                        : dish.Vegetarian == true);
        }
        else
        {
            reqDishes = _context.Dish.Where(dish =>
                (vegeterian == false)
                    ? dish.Vegetarian == true || dish.Vegetarian == false
                    : dish.Vegetarian == true);
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
        var showedDishes = gottedDishes.Skip((pageInfo.CurrentPage - 1) * pageInfo.PageSize).Take(pageInfo.PageSize)
            .ToList();

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

    public Task<DishDTO> GetDishById(Guid id)
    {
        var dish = _context.Dish.FirstOrDefaultAsync(d => d.Id == id);
        if (dish == null)
        {
            throw new Exception(message: "Such dishId not found");
        }

        return dish;
    }

    public async Task<Rating> CheckUserRated(string token, Guid dishId)
    {
        var user = await JwtParseHelper.GetUserFromContext(token, _context);
        
        var rate = await _context.Rating.FindAsync(user.Id,dishId);

        return rate;
    }
    
    public async Task PutUserRating(string token, Guid dishId, double value)
    {
        var user = await JwtParseHelper.GetUserFromContext(token, _context);
        Rating newRate = new Rating { UserId = user.Id, DishId = dishId, Value = value };
        
        var rate = await CheckUserRated(token, dishId);

        if (rate != null)
        {
            rate.Value = value;
        }
        else
        {
            await _context.Rating.AddAsync(new Rating { UserId = user.Id, DishId = dishId, Value = value });
        }

        await _context.SaveChangesAsync();
        
        var avgRate = await _context.Rating.Where(r => r.DishId == dishId).AverageAsync(r => r.Value);
        
        var dish = await _context.Dish.FindAsync(dishId);
        dish.Rating = avgRate;
        
        await _context.SaveChangesAsync();
    }
}