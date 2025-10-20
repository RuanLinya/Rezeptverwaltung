using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.Data;
using RecipeLibrary.Models;

namespace RecipeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController()
        {
            _context = new DataContext();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public IActionResult AddCategory([FromBody] Category category)
        {
            category.Id = Guid.NewGuid();
            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Category updated)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();

            category.Name = updated.Name;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
