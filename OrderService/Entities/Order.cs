namespace OrderService.Entities
{
    public class Order
    {
        public int OrderId { get; set; }

        public Guid UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}