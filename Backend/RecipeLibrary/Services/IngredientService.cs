using System;
using System.Linq;
using System.Collections.Generic;
using RecipeLibrary.Data;
using RecipeLibrary.Models;

namespace RecipeLibrary.Services
{
    /// <summary>
    /// Manages the global list of ingredients.  Ingredients are shared across
    /// all users and recipes.  Ingredient names must be unique.
    /// </summary>
    public class IngredientService
    {
        private readonly DataContext _context;

        public IngredientService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new ingredient with the specified name.  If an ingredient
        /// with the same name already exists (case‑insensitive comparison) the
        /// existing ingredient is returned.  Throws if the name is empty.
        /// </summary>
        public Ingredient AddOrGet(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Ingredient name must not be empty.", nameof(name));

            var existing = _context.Ingredients.FirstOrDefault(i => string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                return existing;
            }

            var ingredient = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = name
            };
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();
            return ingredient;
        }

        /// <summary>
        /// Retrieves an ingredient by its name (case‑insensitive).  Returns
        /// null if no such ingredient exists.
        /// </summary>
        public Ingredient? GetByName(string name)
        {
            return _context.Ingredients.FirstOrDefault(i => string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves an ingredient by its identifier.  Returns null if not
        /// found.
        /// </summary>
        public Ingredient? GetById(Guid id)
        {
            return _context.Ingredients.FirstOrDefault(i => i.Id == id);
        }

        /// <summary>
        /// Returns all ingredients in alphabetical order by name.
        /// </summary>
        public IEnumerable<Ingredient> GetAll()
        {
            return _context.Ingredients.OrderBy(i => i.Name);
        }
    }
}