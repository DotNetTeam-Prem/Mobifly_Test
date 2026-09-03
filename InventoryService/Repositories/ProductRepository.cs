using Dapper;
using InventoryService.Entities;
using InventoryService.IRepositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InventoryService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<int> CreateAsync(Product product)
        {
            using var connection =
                new SqlConnection(_connectionString);

            return await connection.ExecuteScalarAsync<int>(
                "sp_Product_Create",
                new
                {
                    product.Name,
                    product.StockQty,
                    product.Price
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Product?> GetByIdAsync(int productId)
        {
            using var connection =
                new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<Product>(
                "sp_Product_GetById",
                new
                {
                    ProductId = productId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection =
                new SqlConnection(_connectionString);

            return await connection.QueryAsync<Product>(
                "sp_Product_GetAll",
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Product product)
        {
            using var connection =
                new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                "sp_Product_Update",
                new
                {
                    product.ProductId,
                    product.Name,
                    product.StockQty,
                    product.Price
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(int productId)
        {
            using var connection =
                new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                "sp_Product_Delete",
                new
                {
                    ProductId = productId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ReduceStockAsync(
            int productId,
            int quantity)
        {
            using var connection =
                new SqlConnection(_connectionString);

            var rowsAffected =
                await connection.ExecuteScalarAsync<int>(
                    "sp_Product_ReduceStock",
                    new
                    {
                        ProductId = productId,
                        Quantity = quantity
                    },
                    commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }
    }
}