using Microsoft.Data.SqlClient;
using Dapper;

namespace WebApplication4.DataLayer.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
    {
        private readonly string connectionString;

        public GenericRepository(string conn)
        {
            connectionString = conn;
        }
        public void Add(TEntity entity)
        {
            using (var c = new SqlConnection(connectionString))
            {
                var tableName = typeof(TEntity).Name;
                var properties = typeof(TEntity)
                    .GetProperties()
                    .Where(p => !string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase)
                                && !string.Equals(p.Name, "ID", StringComparison.OrdinalIgnoreCase));

                var columnsNames =
                    string.Join(",", properties.Select(x => x.Name));

                var parameterNames =
                    string.Join(",", properties.Select(x => "@" + x.Name));

                var query = $"insert into {tableName} ({columnsNames}) values ({parameterNames})";
                c.Execute(query, entity);

            }
        }

        public void AddOrder(TEntity entity)
        {
            using (var c = new SqlConnection(connectionString))
            {
                var tableName = typeof(TEntity).Name;
                var properties =
              typeof(TEntity).GetProperties();

                var columnsNames =
                    string.Join(",", properties.Select(x => x.Name));

                var parameterNames =
                    string.Join(",", properties.Select(x => "@" + x.Name));

                var query = $"insert into {tableName} ({columnsNames}) values ({parameterNames})";
                c.Execute(query, entity);

            }
        }

        public void Update(TEntity entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var tableName = typeof(TEntity).Name;
                    var properties = typeof(TEntity).GetProperties()
                        .Where(p => !string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase));

                    string setClause = string.Join(",", properties.Select(p => $"{p.Name}=@{p.Name}"));
                    string keyColumn = "ID";

                    string query = $"UPDATE {tableName} SET {setClause} WHERE {keyColumn}=@Id";

                    connection.Execute(query, entity);
                }
            }
            catch (Exception ex)
            {
                // Log or display the error message
                Console.WriteLine("Error occurred: " + ex.Message);
            }
        }
        public List<TEntity> GetAll()
        {
            List<TEntity> entityList = new List<TEntity>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var tableName = typeof(TEntity).Name;
                    string query = $"SELECT * FROM {tableName}";
                    return connection.Query<TEntity>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                // Log or display the error message
                Console.WriteLine("Error occurred: " + ex.Message);
            }

            return entityList;
        }

        public void Delete(object id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var tableName = typeof(TEntity).Name;
                    var idPropertyName = "ID"; // Assuming the primary key property is named "ID"
                    string query = $"DELETE FROM {tableName} WHERE {idPropertyName} = @Id";

                    connection.Execute(query, new { Id = id }); // Corrected parameter name to match query

                }
            }
            catch (Exception ex)
            {
                // Log or display the error message
                Console.WriteLine("Error occurred: " + ex.Message);
            }
        }

    }
}

