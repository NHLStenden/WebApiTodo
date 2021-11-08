using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApiTodo.Models;
using WebApiTodo.Repositories;

namespace WebApiTodo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IEnumerable<Category> Get()
        {
            var categories = _categoryRepository.Get();
            return categories;
        }

        [HttpGet]
        [Route("{id:int}")]
        public Category Get(int id)
        {
            var category = _categoryRepository.Get(id);
            return category;
        }

        [HttpPost]
        public Category Post(Category category)
        {
            var addedCategory = _categoryRepository.Add(category);
            return addedCategory;
        }

        [HttpDelete]
        public bool Delete(int categoryId)
        {
            bool delete = _categoryRepository.Delete(categoryId);
            return delete;
        }
    }
}