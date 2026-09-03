using OrderService.Entities;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(Order order);

        Task<Order?> GetByIdAsync(int orderId);

        Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);

        Task CancelOrderAsync(int orderId);
    }
}