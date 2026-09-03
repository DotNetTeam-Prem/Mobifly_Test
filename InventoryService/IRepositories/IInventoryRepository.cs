using InventoryService.Entities;

namespace InventoryService.IRepositories
{
    public interface IInventoryRepository
    {
        Task<int> CreateAsync(InventoryItem item);

        Task<InventoryItem?> GetByIdAsync(int itemId);

        Task<IEnumerable<InventoryItem>> GetAllAsync();

        Task UpdateAsync(InventoryItem item);

        Task DeleteAsync(int itemId);
    }
}