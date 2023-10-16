using DeliveryApi.Models;

namespace DeliveryApi.Services.BasketService;

public interface IBasketService
{
    public Task<List<BasketDTO>> GetUserBasket(string token);
    public Task AddToUserBasket(string token, Guid dishId);
    public Task DeleteFromUserBasket(string token, Guid dishId);
}