using InventoryService.Entities;
using InventoryService.IServices;
using InventoryService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(Product product)
        {
            var productId = await _service.CreateAsync(product);

            return Ok(new
            {
                ProductId = productId,
                Message = "Product created successfully."
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();

            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            var product = await _service.GetByIdAsync(productId);

            if (product == null)
                return NotFound(new
                {
                    Message = "Product not found."
                });

            return Ok(product);
        }

        [HttpPut("{productId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(
            int productId,
            Product product)
        {
            var existingProduct =
                await _service.GetByIdAsync(productId);

            if (existingProduct == null)
                return NotFound(new
                {
                    Message = "Product not found."
                });

            product.ProductId = productId;

            await _service.UpdateAsync(product);

            return Ok(new
            {
                Message = "Product updated successfully."
            });
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int productId)
        {
            var existingProduct =
                await _service.GetByIdAsync(productId);

            if (existingProduct == null)
                return NotFound(new
                {
                    Message = "Product not found."
                });

            await _service.DeleteAsync(productId);

            return Ok(new
            {
                Message = "Product deleted successfully."
            });
        }

        [HttpPost("{productId}/reduce_stock")]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> ReduceStock(
            int productId,
            int quantity)
        {
            var success =
                await _service.ReduceStockAsync(
                    productId,
                    quantity);

            if (!success)
                return BadRequest(new
                {
                    Message = "Insufficient stock."
                });

            return Ok(new
            {
                Message = "Stock reduced successfully."
            });
        }
    }
}