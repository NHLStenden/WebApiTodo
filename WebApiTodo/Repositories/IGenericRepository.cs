using System.Collections.Generic;

namespace WebApiTodo.Repositories
{
    public interface IGenericRepository<T>
    {
        public List<T> Get();
        public T Get(int id);
        public T Add(T entity);
        public T Update(T entity);
        public bool Delete(int id);
    }
}
