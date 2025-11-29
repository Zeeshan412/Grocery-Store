using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApplication4.Entities;
using Microsoft.EntityFrameworkCore;
using WebApplication4.BusinessLogic.Services;

namespace WebApplication4.Controllers
{
    public class CartController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public CartController(IOrderService orderService, ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        public IActionResult Cart()
        {
            try
            {
                var cart = _cartService.GetCartFromSession(HttpContext.Session);
                return View(cart);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public IActionResult UpdateCart(Dictionary<string, int> quantities)
        {
            try
            {
                if (quantities != null)
                {
                    foreach (var item in quantities)
                    {
                        _cartService.UpdateCartItem(HttpContext.Session, item.Key, item.Value);
                    }
                }
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [Authorize(Policy = "RequireAuthenticatedUser")]
        public ViewResult Checkout()
        {
            var cart = _cartService.GetCartFromSession(HttpContext.Session);
            return View(cart);
        }

        [HttpGet]
        public ViewResult Thankyou()
        {
            var cart = _cartService.GetCartFromSession(HttpContext.Session);
            return View(cart);
        }

        [HttpPost]
        public IActionResult ThankYou(
                string FullName,
                string Address,
                string City,
                string State,
                string ZipCode,
                decimal Total,
                int[]? ProductIDs,
                int[]? Quantities,
                decimal[]? Prices)
        {
            try 
            {
                var userId = User?.Identity?.IsAuthenticated == true
                    ? ((ClaimsIdentity)User.Identity!).FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous"
                    : "Anonymous";

                // Get cart before clearing it
                var cart = _cartService.GetCartFromSession(HttpContext.Session);

                if (ProductIDs == null || Quantities == null || Prices == null || ProductIDs.Length == 0)
                {
                    // If no product IDs, just show thank you page with cart
                    _cartService.ClearCart(HttpContext.Session);
                    return View("Thankyou", cart);
                }

                // Validate arrays have same length
                if (ProductIDs.Length != Quantities.Length || Quantities.Length != Prices.Length)
                {
                    _cartService.ClearCart(HttpContext.Session);
                    return View("Thankyou", cart);
                }

                var order = _orderService.CreateOrder(userId, FullName ?? "Guest", Address ?? "", City ?? "", State ?? "", ZipCode ?? "", Total);
                
                _orderService.ProcessOrderItems(order.ID, ProductIDs, Quantities, Prices);
                
                _cartService.ClearCart(HttpContext.Session);

                return View("Thankyou", cart);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ThankYou: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Try to show thank you page even if order creation fails
                try
                {
                    var cart = _cartService.GetCartFromSession(HttpContext.Session);
                    _cartService.ClearCart(HttpContext.Session);
                    return View("Thankyou", cart);
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your order.");
                }
            }
        }
    }
}
