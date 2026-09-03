using InventoryService.Entities;

namespace InventoryService.Services
{
    public interface IInventoryService
    {
        Task<int> CreateAsync(InventoryItem item);

        Task<InventoryItem?> GetByIdAsync(int itemId);

        Task<IEnumerable<InventoryItem>> GetAllAsync();

        Task UpdateAsync(InventoryItem item);

        Task DeleteAsync(int itemId);
    }
}