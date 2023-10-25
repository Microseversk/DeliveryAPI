using DeliveryApi.Models;

namespace DeliveryApi.Services.OrderService;

public interface IOrderService
{
    public Task<OrderDTO> GetOrderInfo(string token, Guid orderId);
    public Task<List<OrderInfoDTO>> GetOrderList(string token);
    public Task CreateOrder(string token, OrderCreateDTO model);
    public Task ConfirmOrder(string token, Guid orderId);
    
}