namespace WebApplication4.DataLayer.Repositories
{
    public interface IRepository<TEntity>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        List<TEntity> GetAll();
        void Delete(object name);
        void AddOrder(TEntity entity);
    }
}

