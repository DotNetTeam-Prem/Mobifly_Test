using OrderService.Entities;

namespace OrderService.IRepositories
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(Order order);

        Task CreateOrderItemAsync(OrderItem item);

        Task<Order?> GetByIdAsync(int orderId);

        Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);

        Task UpdateStatusAsync(int orderId, string status);
    }
}