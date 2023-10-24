using DeliveryApi.Models;

namespace DeliveryApi.Services.OrderService;

public interface IOrderService
{
    public Task<OrderDTO> GetOrderInfo(string token);
    public Task<OrderInfoDTO> GetOrderList(string token);
    public Task CreateOrder(string token, OrderCreateDTO model);
    public Task ConfirmOrder(string token);
    
}