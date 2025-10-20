using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.Data;
using RecipeLibrary.Models;

namespace RecipeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly DataContext _context;

        public IngredientsController()
        {
            _context = new DataContext();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Ingredients);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var ingredient = _context.Ingredients.FirstOrDefault(i => i.Id == id);
            if (ingredient == null)
                return NotFound();

            return Ok(ingredient);
        }

        [HttpPost]
        public IActionResult AddIngredient([FromBody] Ingredient ingredient)
        {
            ingredient.Id = Guid.NewGuid();
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = ingredient.Id }, ingredient);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Ingredient updated)
        {
            var ingredient = _context.Ingredients.FirstOrDefault(i => i.Id == id);
            if (ingredient == null)
                return NotFound();

            ingredient.Name = updated.Name;
            ingredient.Quantity = updated.Quantity;
            ingredient.Unit = updated.Unit;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var ingredient = _context.Ingredients.FirstOrDefault(i => i.Id == id);
            if (ingredient == null)
                return NotFound();

            _context.Ingredients.Remove(ingredient);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
