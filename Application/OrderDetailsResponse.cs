using Application.Models;

namespace Application
{
    public class OrderDetailsResponse
    {
        public CustomerResponse Customer { get; set; }
        public OrderResponse Order { get; set; }
    }
}
