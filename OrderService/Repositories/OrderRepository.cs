using Dapper;
using Microsoft.Data.SqlClient;
using OrderService.Entities;
using OrderService.IRepositories;
using System.Data;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            using var connection =
                new SqlConnection(_connectionString);

            return await connection.ExecuteScalarAsync<int>(
                "sp_Order_Create",
                new
                {
                    order.UserId,
                    order.TotalAmount,
                    order.Status
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task CreateOrderItemAsync(OrderItem item)
        {
            using var connection =
                new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                "sp_OrderItem_Create",
                new
                {
                    item.OrderId,
                    item.ProductId,
                    item.Quantity,
                    item.Price
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var multi = await connection.QueryMultipleAsync(
                "sp_Order_GetById",
                new
                {
                    OrderId = orderId
                },
                commandType: CommandType.StoredProcedure);

            var order =
                await multi.ReadFirstOrDefaultAsync<Order>();

            if (order != null)
            {
                order.Items =
                    (await multi.ReadAsync<OrderItem>()).ToList();
            }

            return order;
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId)
        {
            using var connection =
                new SqlConnection(_connectionString);

            return await connection.QueryAsync<Order>(
                "sp_Order_GetByUserId",
                new
                {
                    UserId = userId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateStatusAsync(
            int orderId,
            string status)
        {
            using var connection =
                new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                "sp_Order_UpdateStatus",
                new
                {
                    OrderId = orderId,
                    Status = status
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}