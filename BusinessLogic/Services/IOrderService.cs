using WebApplication4.DTOs;

namespace WebApplication4.BusinessLogic.Services
{
    public interface IOrderService
    {
        int GenerateOrderId();
        Order CreateOrder(string userId, string fullName, string address, string city, string state, string zipCode, decimal total);
        void ProcessOrderItems(int orderId, int[] productIds, int[] quantities, decimal[] prices);
        List<Order> GetUserOrders(string userId);
    }
}

