using System.Security.Claims;
using DeliveryApi.Context;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Services.BasketService;

public class BasketService : IBasketService
{
    private readonly DeliveryContext _context;

    public BasketService(DeliveryContext context)
    {
        _context = context;
    }
    public async Task<List<BasketDTO>> GetUserBasket(string token)
    {
        var user = await JwtParseHelper.GetUserFromContext(token, _context);
        var userBasket = _context.Basket.Where(b => b.UserId == user.Id).Include(b => b.Dish).ToList();
        List<BasketDTO> userBasketDTO = new List<BasketDTO>();
        foreach (var item in userBasket)
        {
            userBasketDTO.Add(new BasketDTO
            {
                DishId = item.DishId,
                Name = item.Dish.Name,
                Price = item.Dish.Price,
                Amount = item.Amount,
                TotalPrice = item.Amount * item.Dish.Price,
                Image = item.Dish.Image
            });
        }

        return userBasketDTO;
    }

    public async Task AddToUserBasket(string token, Guid dishId)
    {
        var user = await JwtParseHelper.GetUserFromContext(token, _context);
        var dishCard = await _context.Basket.FirstOrDefaultAsync(b => b.UserId == user.Id && b.DishId == dishId);

        if (dishCard == null)
        {
            await _context.Basket.AddAsync(new Basket { UserId = user.Id, DishId = dishId, Amount = 1 });
        }
        else
        {
            dishCard.Amount += 1;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteFromUserBasket(string token, Guid dishId, bool increase)
    {
        var user = await JwtParseHelper.GetUserFromContext(token, _context);
        var dishCard = await _context.Basket.FirstOrDefaultAsync(b => b.UserId == user.Id && b.DishId == dishId);

        if (increase == false || dishCard.Amount == 1)
        {
            _context.Basket.Remove(dishCard);
        }
        else
        {
            dishCard.Amount -= 1;
        }

        await _context.SaveChangesAsync();
    }
}