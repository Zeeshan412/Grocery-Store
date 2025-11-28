using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.BusinessLogic.Services;

namespace WebApplication4.Controllers
{
    [Authorize(Policy = "AdminAccess")]
    [Authorize(Policy = "RequireAuthenticatedUser")]
    public class DeleteCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public DeleteCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public ViewResult DeleteCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            _categoryService.DeleteCategory(id);
            TempData["CategoryMessage"] = $"Category {id} deleted.";
            return View();
        }
    }
}
