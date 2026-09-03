using InventoryService.Entities;
using InventoryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryItem item)
        {
            var itemId = await _service.CreateAsync(item);

            return Ok(new
            {
                ItemId = itemId,
                Message = "Item created successfully."
            });
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetById(int itemId)
        {
            var item = await _service.GetByIdAsync(itemId);

            if (item == null)
                return NotFound(new
                {
                    Message = "Item not found."
                });

            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();

            return Ok(items);
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> Update(
            int itemId,
            InventoryItem item)
        {
            item.ItemId = itemId;

            var existingItem =
                await _service.GetByIdAsync(itemId);

            if (existingItem == null)
                return NotFound(new
                {
                    Message = "Item not found."
                });

            await _service.UpdateAsync(item);

            return Ok(new
            {
                Message = "Item updated successfully."
            });
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> Delete(int itemId)
        {
            var existingItem =
                await _service.GetByIdAsync(itemId);

            if (existingItem == null)
                return NotFound(new
                {
                    Message = "Item not found."
                });

            await _service.DeleteAsync(itemId);

            return Ok(new
            {
                Message = "Item deleted successfully."
            });
        }
    }
}