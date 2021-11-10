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

        [HttpGet("{id:int}")]
        //[Route("{id:int}")]
        public ActionResult<Category> Get(int id)
        {
            var category = _categoryRepository.Get(id);
            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public ActionResult<Category> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addedCategory = _categoryRepository.Add(category);
            return CreatedAtAction(nameof(Get), new {id = addedCategory.Id}, addedCategory);
        }

        [HttpPut("{categoryId:int}")]
        public IActionResult Put(int categoryId, Category input)
        {
            if (categoryId != input.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Category category = _categoryRepository.Get(categoryId);
            // if (category is null)
            // {
            //     return NotFound();
            // }

            Category updatedCategory = _categoryRepository.Update(input);

            return updatedCategory is not null ? Ok(updatedCategory) : BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete(int categoryId)
        {
            var category = _categoryRepository.Get(categoryId); //dubbel werk, zie ook Delete()
            if (category is null)
            {
                return NotFound();
            }

            var removedCategory = _categoryRepository.Delete(categoryId);
            return removedCategory is not null ? Ok() : BadRequest();
        }
    }
}