using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using System.Linq;
using WebApplication4.DTOs;
using WebApplication4.BusinessLogic.Services;

namespace WebApplication4.Controllers
{
    [Authorize(Policy = "CustomerAccess")]
    [Authorize(Policy = "RequireAuthenticatedUser")]
    public class DashBoardController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public DashBoardController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        [HttpGet]
        public ViewResult Dashboard(string searchQuery = "")
        {
            var categoryProducts = _productService.GetCategoryProducts();
            
            // Filter products by search query if provided
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var filteredProducts = categoryProducts.Products
                    .Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                
                categoryProducts.Products = filteredProducts;
            }
            
            ViewBag.SearchQuery = searchQuery;
            return View(categoryProducts);
        }

        [HttpPost]
        public IActionResult Dashboard(string ProductName, int Price, int Stock, string ImageUrl)
        {
            var product = new Product
            {
                Name = ProductName,
                Price = Price,
                Stock = Stock,
                ImageUrl = ImageUrl
            };

            _cartService.AddToCart(HttpContext.Session, product, Stock);
            
            // Set success message
            TempData["CartMessage"] = $"{ProductName} added to cart successfully!";
            
            var categoryProducts = _productService.GetCategoryProducts();
            return View(categoryProducts);
        }

    }
}