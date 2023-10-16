using DeliveryApi.Models;

namespace DeliveryApi.Services.BasketService;

public interface IBasketService
{
    public Task<List<BasketDTO>> GetUserBasket(string token);
    public Task AddToUserBasket(string token);
    public Task DeleteFromUserBasket(string token);
}