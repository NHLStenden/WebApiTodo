using WebApiTodo.Models;

namespace WebApiTodo.Repositories
{
    public class TodoRepository : GenericRepository<Todo>, ITodoRepository
    {
        public TodoRepository(TodoContext db) : base(db)
        {
        }
    }
}