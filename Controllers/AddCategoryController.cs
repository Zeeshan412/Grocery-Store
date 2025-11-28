using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.BusinessLogic.Services;

namespace WebApplication4.Controllers
{
    [Authorize(Policy = "AdminAccess")]
    [Authorize(Policy = "RequireAuthenticatedUser")]
    public class AddCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public AddCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(string Name)
        {
            _categoryService.AddCategory(Name);
            TempData["CategoryMessage"] = $"{Name} added successfully.";
            return View();
        }
    }
}
