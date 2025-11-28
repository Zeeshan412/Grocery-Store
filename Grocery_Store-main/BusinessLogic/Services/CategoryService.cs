using WebApplication4.Entities;
using WebApplication4.DataLayer.Repositories;

namespace WebApplication4.BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(string connectionString)
        {
            _categoryRepository = new GenericRepository<Category>(connectionString);
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }

        public Category? GetCategoryById(int id)
        {
            return GetAllCategories().FirstOrDefault(c => c.Id == id);
        }

        public void AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty", nameof(name));
            }

            var category = new Category { Name = name };
            _categoryRepository.Add(category);
        }

        public void UpdateCategory(int id, string name)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid category id", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty", nameof(name));
            }

            var category = new Category { Id = id, Name = name };
            _categoryRepository.Update(category);
        }

        public void DeleteCategory(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid category id", nameof(id));
            }

            _categoryRepository.Delete(id);
        }
    }
}

