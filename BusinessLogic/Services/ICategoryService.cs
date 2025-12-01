using WebApplication4.DTOs;

namespace WebApplication4.BusinessLogic.Services
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        Category? GetCategoryById(int id);
        void AddCategory(string name);
        void UpdateCategory(int id, string name);
        void DeleteCategory(int id);
    }
}

