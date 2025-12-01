using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTOs;
using WebApplication4.BusinessLogic.Services;

namespace WebApplication4.Controllers
{
    [Authorize(Policy = "AdminAccess")]
    [Authorize(Policy = "RequireAuthenticatedUser")]
    public class DeleteProductController : Controller
    {
        private readonly IProductService _productService;

        public DeleteProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult DeleteProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteProduct(int ID)
        {
            _productService.DeleteProduct(ID);
            return View();
        }
    }
}
