namespace Application.Models
{
    public class OrderItemResponse
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal PriceEach { get; set; }
        public bool ContainsGift { get; set; }
    }
}
