using System.Linq;
using WebApplication4.DTOs;
using WebApplication4.DataLayer.Repositories;

namespace WebApplication4.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;

        public ProductService(string connectionString)
            : this(new GenericRepository<Product>(connectionString), new GenericRepository<Category>(connectionString))
        {
        }

        public ProductService(IRepository<Product> productRepository, IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }

        public CategoryProducts GetCategoryProducts()
        {
            // Business Logic: Combine products and categories
            return new CategoryProducts
            {
                Products = GetAllProducts(),
                Category = GetAllCategories()
            };
        }

        public void AddProduct(Product product)
        {
            // Business Logic: Validate product before adding
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name cannot be empty");
            }

            if (product.Price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero");
            }

            if (product.Stock < 0)
            {
                throw new ArgumentException("Product stock cannot be negative");
            }

            _productRepository.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            // Business Logic: Validate product before updating
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name cannot be empty");
            }

            _productRepository.Update(product);
        }

        public void DeleteProduct(int productId)
        {
            // Business Logic: Validate product ID before deleting
            if (productId <= 0)
            {
                throw new ArgumentException("Invalid product ID");
            }

            _productRepository.Delete(productId);
        }

        public Product? GetProductByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var products = GetAllProducts();
            return products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsProductInStock(string productName, int requestedQuantity)
        {
            // Business Logic: Check if product has sufficient stock
            var product = GetProductByName(productName);
            if (product == null)
            {
                return false;
            }

            return product.Stock >= requestedQuantity;
        }
    }
}

