using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DesafioWebApi.Repositories
{
    public abstract class AbstractRepository<T>
    {
        private string _connectionString;
        protected string ConnectionString => _connectionString;
        public AbstractRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        }
        public abstract bool Create(T item);
        public abstract bool Delete(int id);
        public abstract bool Update(T item);
        public abstract T FindById(int id);
        public abstract T GetLastInserted();
        public abstract IEnumerable<T> GetAll();
    }
}
