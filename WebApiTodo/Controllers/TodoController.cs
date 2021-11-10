using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiTodo.Models;
using WebApiTodo.Repositories;

namespace WebApiTodo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public IEnumerable<Todo> Get()
        {
            var todos = _todoRepository.Get();
            return todos;
        }

        [HttpGet]
        [Route("{id:int}")]
        public Todo Get(int id)
        {
            var todo = _todoRepository.Get(id);
            return todo;
        }

        [HttpPost]
        public Todo Post(Todo todo)
        {
            var addedTodo = _todoRepository.Add(todo);
            return addedTodo;
        }

        [HttpDelete]
        public bool Delete(int todoId)
        {
            bool delete = _todoRepository.Delete(todoId);
            return delete;
        }
    }
}