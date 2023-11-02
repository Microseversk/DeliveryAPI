using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Exceptions;
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


    public async Task<OrderDTO> GetOrderInfo(string token, Guid orderId)
    {
        var user = await JwtTokenParseHelper.GetUserFromContext(token, _dContext);

        if (user == null)
        {
            throw new BadRequestException("Invalid token");
        }

        var order = await _dContext.Order.FirstOrDefaultAsync(o => o.UserId == user.Id && o.Id == orderId);

        if (order == null)
        {
            throw new BadRequestException("Order not found");
        }

        var orderDishes = (from od in _dContext.OrderDishes
            where od.OrderId == orderId
            select new BasketDTO
            {
                DishId = od.DishId,
                Name = od.Dish.Name,
                Price = od.Dish.Price,
                Amount = od.Amount,
                TotalPrice = od.Amount * od.Dish.Price,
                Image = od.Dish.Image
            }).ToList();
        return new OrderDTO
        {
            Id = order.Id,
            DeliveryTime = order.DeliveryTime,
            OrderTime = order.OrderTime,
            Status = order.Status,
            Price = order.Price,
            Dishes = orderDishes,
            Address = order.Address.ToString()
            //todo "переделать на название адреса вместо айди"
        };
    }

    public async Task<List<OrderInfoDTO>> GetOrderList(string token)
    {
        var user = await JwtTokenParseHelper.GetUserFromContext(token, _dContext);

        if (user == null)
        {
            throw new BadRequestException("Invalid token");
        }

        var userOrders = _dContext.Order.Where(o => o.UserId == user.Id);
        if (userOrders.IsNullOrEmpty())
        {
            throw new NotFoundException("User has no orders");
        }

        List<OrderInfoDTO> userOrdersDTO = new List<OrderInfoDTO>();
        foreach (var order in userOrders)
        {
            userOrdersDTO.Add(new OrderInfoDTO
            {
                Id = order.Id,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Status = order.Status,
                Price = order.Price
            });
        }

        return userOrdersDTO;
    }

    public async Task CreateOrder(string token, OrderCreateDTO model)
    {
        var user = await JwtTokenParseHelper.GetUserFromContext(token, _dContext);

        if (user == null)
        {
            throw new BadRequestException("Invalid token");
        }

        var userBasket = _dContext.Basket.Where(b => b.UserId == user.Id).Include(b => b.Dish);
        if (userBasket.IsNullOrEmpty())
        {
            throw new BadRequestException("Denied. Basket is empty");
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

    public async Task ConfirmOrder(string token, Guid orderId)
    {
        var order = await _dContext.Order.FindAsync(orderId);
        if (order.Status == Status.Delivered)
        {
            throw new BadRequestException("Denied. Order was delivered");
        }

        order.Status = Status.Delivered;
        await _dContext.SaveChangesAsync();
    }
}