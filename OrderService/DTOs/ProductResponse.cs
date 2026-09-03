namespace OrderService.DTOs
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StockQty { get; set; }
        public decimal Price { get; set; }
    }
}