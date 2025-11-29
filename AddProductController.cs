using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WebApplication4.Entities;
using System.Linq;
using WebApplication4.BusinessLogic.Services;

namespace WebApplication4.Controllers
{
    [Authorize(Policy = "AdminAccess")]
    [Authorize(Policy = "RequireAuthenticatedUser")]
    public class AddProductController : Controller
    {
        private readonly ILogger<AddProductController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;

        public AddProductController(ILogger<AddProductController> logger, IWebHostEnvironment env, IProductService productService)
        {
            _logger = logger;
            _env = env;
            _productService = productService;
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            var categoryProducts = new CategoryProducts
            {
                Category = _productService.GetAllCategories()
            };
            return View(categoryProducts);
        }

        [HttpPost]
        public ActionResult AddProduct(string Name, int CategoryID, int Price, int Stock, IFormFile xyz)
        {
            string wwwFolder = _env.WebRootPath;
            string path = Path.Combine(wwwFolder, "UploadedImages");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = Path.GetFileName(xyz.FileName);
            var pathWithFileName = Path.Combine(path, filename);
            using (var stream = new FileStream(pathWithFileName, FileMode.Create))
            {
                xyz.CopyTo(stream);
            }
            
            int index = pathWithFileName.IndexOf("Uploaded");
            pathWithFileName = pathWithFileName.Substring(index);
            pathWithFileName = pathWithFileName.Replace('\\', '/');
            pathWithFileName = "/" + pathWithFileName;

            var product = new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                CategoryId = CategoryID,
                ImageUrl = pathWithFileName
            };

            _productService.AddProduct(product);

            var categoryProducts = new CategoryProducts
            {
                Category = _productService.GetAllCategories()
            };
            return View(categoryProducts);
        }
        
    }
}
