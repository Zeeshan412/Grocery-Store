using WebApplication4.Entities;
using WebApplication4.DataLayer.Repositories;

namespace WebApplication4.BusinessLogic.Factories
{
    /// <summary>
    /// Factory Pattern Implementation
    /// Creates repository instances without exposing the instantiation logic
    /// </summary>
    public class RepositoryFactory
    {
        private readonly string _connectionString;

        public RepositoryFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Creates a generic repository for the specified entity type
        /// </summary>
        public IRepository<T> CreateRepository<T>() where T : class
        {
            return new GenericRepository<T>(_connectionString);
        }

        /// <summary>
        /// Creates a product repository
        /// </summary>
        public IRepository<Product> CreateProductRepository()
        {
            return new GenericRepository<Product>(_connectionString);
        }

        /// <summary>
        /// Creates a category repository
        /// </summary>
        public IRepository<Category> CreateCategoryRepository()
        {
            return new GenericRepository<Category>(_connectionString);
        }

        /// <summary>
        /// Creates an order repository
        /// </summary>
        public IRepository<Order> CreateOrderRepository()
        {
            return new GenericRepository<Order>(_connectionString);
        }
    }
}

