using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.BusinessLogic.Services;
using WebApplication4.Entities;

namespace WebApplication4.Controllers
{
    [Authorize(Policy = "AdminAccess")]
    [Authorize(Policy = "RequireAuthenticatedUser")]
    public class AdminAddController : Controller
    {
        private readonly ILogger<AdminAddController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;

        public AdminAddController(
            ILogger<AdminAddController> logger,
            IWebHostEnvironment env,
            IProductService productService)
        {
            _logger = logger;
            _env = env;
            _productService = productService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddData(string Name, int Price, int Stock, IFormFile xyz)
        {
            var product = new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                ImageUrl = SaveImage(xyz)
            };
            _productService.AddProduct(product);
            TempData["ProductMessage"] = $"{Name} added successfully.";
            return View("Add");
        }

        private string SaveImage(IFormFile file)
        {
            string wwwFolder = _env.WebRootPath;
            string path = Path.Combine(wwwFolder, "UploadedImages");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = Path.GetFileName(file.FileName);
            var pathWithFileName = Path.Combine(path, filename);
            using (var stream = new FileStream(pathWithFileName, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            int index = pathWithFileName.IndexOf("Uploaded", StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return $"/UploadedImages/{file.FileName}";
            }
            var normalizedPath = pathWithFileName.Substring(index).Replace('\\', '/');
            return "/" + normalizedPath;
        }
    }
}
