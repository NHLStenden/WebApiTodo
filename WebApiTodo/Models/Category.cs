using System.ComponentModel.DataAnnotations;

namespace WebApiTodo.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(100)]
        public string Name { get; set; }
    }
}