using OrderService.DTOs;

namespace OrderService.IServices
{
    public interface IProductClient
    {
        Task<ProductResponse?> GetProductAsync(int productId);
        Task<bool> ReduceStockAsync(int productId, int quantity);
    }
}