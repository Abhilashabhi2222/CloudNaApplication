using Application;
using Application.Interfaces;
using Application.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public OrderDetailsResponse GetMostRecentOrder(string email, string customerId)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var query = @"
                                SELECT
                                    c.CustomerId,
                                    c.FirstName,
                                    c.LastName,
                                    c.Email,
                                    c.HouseNo,
                                    c.Street,
                                    c.Town,
                                    c.PostCode,
                                    o.OrderId,
                                    o.OrderDate,
                                    o.DeliveryExpected,
                                    o.ContainsGift,
                                    oi.OrderItemId,
                                    oi.ProductId,
                                    oi.Quantity,
                                    oi.Price
                                FROM
                                    Customers c
                                JOIN
                                    Orders o ON c.CustomerId = o.CustomerId
                                LEFT JOIN
                                    OrderItems oi ON o.OrderId = oi.OrderId
                                WHERE
                                    c.Email = @Email
                                    AND c.CustomerId = @CustomerId
                                ORDER BY
                                    o.OrderDate DESC;";

                var orderDictionary = new Dictionary<int, OrderDetailsResponse>();

                var result = dbConnection.Query<OrderDetailsResponse, OrderItemResponse, OrderDetailsResponse>(
                query,
                (orderDetails, orderItem) =>
                {
                    if (orderDetails != null)
                    {
                        if (!orderDictionary.TryGetValue(orderDetails.Order.OrderNumber, out var existingOrder))
                        {
                            existingOrder = orderDetails;
                            orderDictionary.Add(existingOrder.Order.OrderNumber, existingOrder);
                        }

                        if (orderItem != null)
                        {
                            existingOrder.Order.OrderItems.Add(orderItem);
                        }
                    }
                    return orderDetails;
                },

                new { Email = email, CustomerId = customerId },
                    splitOn: "OrderItemId"
                    ).Distinct().ToList();

                return result.FirstOrDefault();
            }
        }
    }
}