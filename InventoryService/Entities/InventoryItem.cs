namespace InventoryService.Entities
{
    public class InventoryItem
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}