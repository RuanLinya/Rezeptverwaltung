using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.Data;
using RecipeLibrary.Models;

namespace RecipeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly DataContext _context;

        public RecipesController()
        {
            _context = new DataContext();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Recipes);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null) return NotFound();
            return Ok(recipe);
        }

        [HttpPost]
        public IActionResult AddRecipe([FromBody] Recipe recipe)
        {
            recipe.Id = Guid.NewGuid();
            _context.Recipes.Add(recipe);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = recipe.Id }, recipe);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Recipe updated)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null) return NotFound();

            recipe.Title = updated.Title;
            recipe.Description = updated.Description;
            recipe.CategoryId = updated.CategoryId;
            recipe.Ingredients = updated.Ingredients;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null) return NotFound();

            _context.Recipes.Remove(recipe);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
