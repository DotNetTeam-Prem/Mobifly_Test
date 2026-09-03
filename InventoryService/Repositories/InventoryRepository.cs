using Dapper;
using InventoryService.Entities;
using InventoryService.IRepositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InventoryService.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly string _connectionString;

        public InventoryRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<int> CreateAsync(InventoryItem item)
        {
            using var connection =
                new SqlConnection(_connectionString);

            return await connection.ExecuteScalarAsync<int>(
                "sp_InventoryItem_Create",
                new
                {
                    item.ItemName,
                    item.Category,
                    item.Quantity
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<InventoryItem?> GetByIdAsync(int itemId)
        {
            using var connection =
                new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<InventoryItem>(
                "sp_InventoryItem_GetById",
                new
                {
                    ItemId = itemId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InventoryItem>> GetAllAsync()
        {
            using var connection =
                new SqlConnection(_connectionString);

            return await connection.QueryAsync<InventoryItem>(
                "sp_InventoryItem_GetAll",
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(InventoryItem item)
        {
            using var connection =
                new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                "sp_InventoryItem_Update",
                new
                {
                    item.ItemId,
                    item.ItemName,
                    item.Category,
                    item.Quantity,
                    item.IsActive
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(int itemId)
        {
            using var connection =
                new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                "sp_InventoryItem_Delete",
                new
                {
                    ItemId = itemId
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}