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
        var email = JwtParseHelper.GetClaimValue(token, ClaimTypes.Email);
        var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
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

    public Task AddToUserBasket(string token)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFromUserBasket(string token)
    {
        throw new NotImplementedException();
    }
}