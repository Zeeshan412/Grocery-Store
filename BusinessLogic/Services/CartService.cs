using System.Linq;
using Newtonsoft.Json;
using WebApplication4.Entities;
using Microsoft.AspNetCore.Http;

namespace WebApplication4.BusinessLogic.Services
{
    public class CartService : ICartService
    {
        private const string CartSessionKey = "Cart";

        public List<Product> GetCartFromSession(ISession session)
        {
            // Business Logic: Retrieve cart from session or return empty list
            string? cartJson = session.GetString(CartSessionKey);
            if (!string.IsNullOrEmpty(cartJson))
            {
                return JsonConvert.DeserializeObject<List<Product>>(cartJson) ?? new List<Product>();
            }
            return new List<Product>();
        }

        public void AddToCart(ISession session, Product product, int quantity)
        {
            // Business Logic: Add product to cart or update quantity if already exists
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var cart = GetCartFromSession(session);

            // Check if product already exists in cart
            var existingItem = cart.FirstOrDefault(p => p.Name == product.Name);
            if (existingItem != null)
            {
                // Update quantity if product already in cart
                existingItem.Stock += quantity;
            }
            else
            {
                // Add new product to cart
                var cartProduct = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Stock = quantity,
                    ImageUrl = product.ImageUrl
                };
                cart.Add(cartProduct);
            }

            SaveCartToSession(session, cart);
        }

        public void UpdateCartItem(ISession session, string productName, int quantity)
        {
            // Business Logic: Update quantity of specific cart item
            var cart = GetCartFromSession(session);
            var item = cart.FirstOrDefault(p => p.Name == productName);
            
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Stock = quantity;
                }
                SaveCartToSession(session, cart);
            }
        }

        public void RemoveFromCart(ISession session, string productName)
        {
            // Business Logic: Remove specific product from cart
            var cart = GetCartFromSession(session);
            var item = cart.FirstOrDefault(p => p.Name == productName);
            
            if (item != null)
            {
                cart.Remove(item);
                SaveCartToSession(session, cart);
            }
        }

        public void ClearCart(ISession session)
        {
            // Business Logic: Clear entire cart
            session.Remove(CartSessionKey);
        }

        public decimal CalculateTotal(List<Product> cart)
        {
            // Business Logic: Calculate total price of all items in cart
            if (cart == null || !cart.Any())
            {
                return 0;
            }

            return cart.Sum(item => item.Price * item.Stock);
        }

        public bool IsCartEmpty(ISession session)
        {
            // Business Logic: Check if cart is empty
            var cart = GetCartFromSession(session);
            return cart == null || !cart.Any();
        }

        private void SaveCartToSession(ISession session, List<Product> cart)
        {
            // Helper method: Save cart to session
            string cartJson = JsonConvert.SerializeObject(cart);
            session.SetString(CartSessionKey, cartJson);
        }
    }
}

