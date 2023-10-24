using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Helpers;
using DeliveryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryApi.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly DeliveryContext _dContext;
    private readonly AddressContext _aContext;

    public OrderService(DeliveryContext dContext, AddressContext aContext)
    {
        _dContext = dContext;
        _aContext = aContext;
    }


    public Task<OrderDTO> GetOrderInfo(string token)
    {
        throw new NotImplementedException();
    }

    public Task<OrderInfoDTO> GetOrderList(string token)
    {
        throw new NotImplementedException();
    }

    public async Task CreateOrder(string token, OrderCreateDTO model)
    {
        var user = await JwtTokenParseHelper.GetUserFromContext(token, _dContext);
        var userBasket = _dContext.Basket.Where(b => b.UserId == user.Id).Include(b => b.Dish);
        if (userBasket.IsNullOrEmpty())
        {
            throw new Exception(message: "Basket is empty");
        }

        var orderId = Guid.NewGuid();
        double price = 0;
        foreach (var item in userBasket)
        {
            price += item.Dish.Price * item.Amount;
            await _dContext.AddAsync(new OrderDishes
            {
                DishId = item.DishId,
                OrderId = orderId,
                Amount = item.Amount
            });
            _dContext.Basket.Remove(item);
        }

        await _dContext.Order.AddAsync(new Order
        {
            Id = orderId,
            UserId = user.Id,
            User = user,
            OrderTime = DateTime.UtcNow,
            DeliveryTime = model.DeliveryTime,
            Status = Status.InProcess,
            Price = price,
            Address = model.AddressId.ToString()
        });
        await _dContext.SaveChangesAsync();
    }

    public Task ConfirmOrder(string token)
    {
        throw new NotImplementedException();
    }
}