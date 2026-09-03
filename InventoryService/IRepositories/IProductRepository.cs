using InventoryService.Entities;

namespace InventoryService.IRepositories
{
    public interface IProductRepository
    {
        Task<int> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(int productId);
        Task<IEnumerable<Product>> GetAllAsync();
        Task UpdateAsync(Product product);
        Task DeleteAsync(int productId);
        Task<bool> ReduceStockAsync(int productId, int quantity);
    }
}