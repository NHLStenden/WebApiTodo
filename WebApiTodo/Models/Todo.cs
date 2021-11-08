using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiTodo.Models
{
    public class Todo
    {
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(150)]
        public string Description { get; set; }
        public bool Completed { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}