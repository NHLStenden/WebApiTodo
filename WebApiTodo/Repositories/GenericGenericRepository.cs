using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApiTodo.Models;

namespace WebApiTodo.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext Db;

        public GenericRepository(TodoContext db)
        {
            Db = db;
        }

        protected IQueryable<T> AsQueryable()
        {
            return Db.Set<T>().AsQueryable();
        }

        public List<T> Get()
        {
            return Db.Set<T>().ToList();
        }

        public T Get(int id)
        {
            return Db.Set<T>().Find(id);
        }

        public T Add(T entity)
        {
            Db.Set<T>().Add(entity);
            Db.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            // if (Db.Set<T>().Find(id) is T foundEntity)
            // {
                Db.Set<T>().Update(entity);
                Db.SaveChanges();
            //     return todo;
            // }
            
            
            
            return entity;
        }

        public T Delete(int id)
        {
            if (Db.Find<T>(id) is T todo)
            {
                Db.Set<T>().Remove(todo);
                Db.SaveChanges();
                return todo;
            }

            return null;
        }
    }
}
