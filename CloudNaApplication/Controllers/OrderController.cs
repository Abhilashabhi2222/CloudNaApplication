using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudNa.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        [HttpPost]
        [Route("api/GetOrders")]
        public IActionResult GetRecentOrder([FromBody] CustomerRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request. Request body is null.");
            }

            var orderDetails = _orderRepository.GetMostRecentOrder(request.Email, request.CustomerId);

            if (orderDetails == null || orderDetails.Customer == null)
            {
                return BadRequest("Invalid request. Email address does not match customer ID.");
            }
            if (orderDetails.Order != null && orderDetails.Order.OrderItems.Any(item => item.ContainsGift))
            {
                foreach (var item in orderDetails.Order.OrderItems.Where(item => item.ContainsGift))
                {
                    item.Product = "Gift";
                }
            }
            return Ok(orderDetails);
        }
    }
}
