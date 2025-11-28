using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.BusinessLogic.Services;

namespace WebApplication4.Controllers
{
    [Authorize(Policy = "AdminAccess")]
    [Authorize(Policy = "RequireAuthenticatedUser")]
    public class UpdateCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public UpdateCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult UpdateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCategory(int id, string Name)
        {
            _categoryService.UpdateCategory(id, Name);
            TempData["CategoryMessage"] = $"Category {id} updated.";
            return View();
        }
    }
}
