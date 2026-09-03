using InventoryService.Entities;
using InventoryService.IRepositories;
using InventoryService.IServices;

namespace InventoryService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(Product product)
        {
            if (product.StockQty < 0)
                throw new Exception("Stock quantity cannot be negative.");

            if (product.Price < 0)
                throw new Exception("Price cannot be negative.");

            return await _repository.CreateAsync(product);
        }

        public async Task<Product?> GetByIdAsync(int productId)
        {
            return await _repository.GetByIdAsync(productId);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            if (product.StockQty < 0)
                throw new Exception("Stock quantity cannot be negative.");

            if (product.Price < 0)
                throw new Exception("Price cannot be negative.");

            await _repository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int productId)
        {
            await _repository.DeleteAsync(productId);
        }

        public async Task<bool> ReduceStockAsync(
            int productId,
            int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than zero.");

            return await _repository.ReduceStockAsync(
                productId,
                quantity);
        }
    }
}