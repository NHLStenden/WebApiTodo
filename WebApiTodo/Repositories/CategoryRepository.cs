using WebApiTodo.Models;

namespace WebApiTodo.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(TodoContext db) : base(db)
        {
        }
    }
}