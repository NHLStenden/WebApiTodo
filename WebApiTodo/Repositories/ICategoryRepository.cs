using WebApiTodo.Models;

namespace WebApiTodo.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        
    }

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(TodoContext db) : base(db)
        {
        }
    }
}