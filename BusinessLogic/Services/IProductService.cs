using WebApplication4.Entities;

namespace WebApplication4.BusinessLogic.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        List<Category> GetAllCategories();
        CategoryProducts GetCategoryProducts();
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int productId);
        Product? GetProductByName(string name);
        bool IsProductInStock(string productName, int requestedQuantity);
    }
}

