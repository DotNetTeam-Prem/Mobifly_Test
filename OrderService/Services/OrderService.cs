using OrderService.Entities;
using OrderService.IRepositories;
using OrderService.Repositories;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository repository,
            ILogger<OrderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            var orderId = await _repository.CreateOrderAsync(order);

            foreach (var item in order.Items)
            {
                item.OrderId = orderId;

                await _repository.CreateOrderItemAsync(item);
            }

            _logger.LogInformation(
                "Order created successfully: {OrderId}",
                orderId);

            return orderId;
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _repository.GetByIdAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _repository.GetByIdAsync(orderId);

            if (order == null)
                throw new Exception("Order not found.");

            if (order.Status == "CANCELLED")
                throw new Exception("Order is already cancelled.");

            await _repository.UpdateStatusAsync(
                orderId,
                "CANCELLED");
        }
    }
}