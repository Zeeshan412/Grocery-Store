using WebApplication4.DTOs;

namespace WebApplication4.BusinessLogic.Services
{
    public interface ICartService
    {
        List<Product> GetCartFromSession(ISession session);
        void AddToCart(ISession session, Product product, int quantity);
        void UpdateCartItem(ISession session, string productName, int quantity);
        void RemoveFromCart(ISession session, string productName);
        void ClearCart(ISession session);
        decimal CalculateTotal(List<Product> cart);
        bool IsCartEmpty(ISession session);
    }
}

