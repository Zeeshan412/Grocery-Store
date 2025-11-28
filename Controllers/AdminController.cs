using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.BusinessLogic.Services;

namespace WebApplication4.Controllers
{
    [Authorize(Policy = "AdminAccess")]
    [Authorize(Policy = "RequireAuthenticatedUser")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public ActionResult Admin()
        {
            ViewData["Title"] = "Admin Dashboard";
            
            var products = _productService.GetAllProducts();
            var categories = _categoryService.GetAllCategories();
            
            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalCategories = categories.Count;
            ViewBag.LowStockProducts = products.Count(p => p.Stock < 10);
            
            return View();
        }
    }
}
