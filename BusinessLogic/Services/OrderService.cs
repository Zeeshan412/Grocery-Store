using System.Linq;
using WebApplication4.DTOs;
using WebApplication4.DataLayer.Repositories;

namespace WebApplication4.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderProducts> _orderProductsRepository;
        private readonly string _connectionString;

        public OrderService(string connectionString)
        {
            _connectionString = connectionString;
            _orderRepository = new GenericRepository<Order>(connectionString);
            _orderProductsRepository = new GenericRepository<OrderProducts>(connectionString);
        }

        public int GenerateOrderId()
        {
            // Business Logic: Get the maximum order ID and increment by 1
            var orders = _orderRepository.GetAll();
            if (orders == null || !orders.Any())
            {
                return 1; // First order
            }
            return orders.Max(o => o.ID) + 1;
        }

        public Order CreateOrder(string userId, string fullName, string address, string city, string state, string zipCode, decimal total)
        {
            // Business Logic: Create order with current date and user information
            var order = new Order
            {
                ID = GenerateOrderId(),
                UID = userId ?? "Anonymous",
                Name = fullName,
                Address = address,
                City = city,
                State = state,
                ZipCode = zipCode,
                Bill = total,
                OrderDate = DateTime.Now
            };

            _orderRepository.AddOrder(order);
            return order;
        }

        public void ProcessOrderItems(int orderId, int[] productIds, int[] quantities, decimal[] prices)
        {
            // Business Logic: Process each order item
            if (productIds == null || quantities == null || prices == null)
            {
                throw new ArgumentException("Order items cannot be null");
            }

            if (productIds.Length != quantities.Length || quantities.Length != prices.Length)
            {
                throw new ArgumentException("Order item arrays must have the same length");
            }

            for (int i = 0; i < productIds.Length; i++)
            {
                var orderProduct = new OrderProducts
                {
                    OrderID = orderId,
                    ProductID = productIds[i],
                    Quantity = quantities[i],
                    Price = prices[i]
                };
                _orderProductsRepository.AddOrder(orderProduct);
            }
        }

        public List<Order> GetUserOrders(string userId)
        {
            // Business Logic: Retrieve all orders for a specific user
            var allOrders = _orderRepository.GetAll();
            return allOrders.Where(o => o.UID == userId).ToList();
        }
    }
}

