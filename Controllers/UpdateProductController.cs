using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTOs;
using WebApplication4.BusinessLogic.Services;

namespace WebApplication4.Controllers
{
    [Authorize(Policy = "AdminAccess")]
    [Authorize(Policy = "RequireAuthenticatedUser")]
    public class UpdateProductController : Controller
    {
        private readonly IProductService _productService;

        public UpdateProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult UpdateProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateProduct(string Name, int Price, int Stock)
        {
            var product = new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock
            };
            _productService.UpdateProduct(product);
            return View();
        }
    }
}
