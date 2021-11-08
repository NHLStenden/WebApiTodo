using Microsoft.EntityFrameworkCore;

namespace WebApiTodo.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Category> Categories { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
            
        }
    }
}