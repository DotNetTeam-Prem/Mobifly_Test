using InventoryService.Entities;
using InventoryService.IRepositories;
using InventoryService.Repositories;

namespace InventoryService.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(
            IInventoryRepository repository,
            ILogger<InventoryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int> CreateAsync(InventoryItem item)
        {
            var itemId = await _repository.CreateAsync(item);

            _logger.LogInformation(
                "Inventory item created: {ItemId}",
                itemId);

            return itemId;
        }

        public async Task<InventoryItem?> GetByIdAsync(int itemId)
        {
            return await _repository.GetByIdAsync(itemId);
        }

        public async Task<IEnumerable<InventoryItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task UpdateAsync(InventoryItem item)
        {
            await _repository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int itemId)
        {
            await _repository.DeleteAsync(itemId);
        }
    }
}