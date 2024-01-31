namespace Application.Models
{
    public class OrderResponse
    {
        public int OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; }
        public string DeliveryExpected { get; set; }
    }
}
