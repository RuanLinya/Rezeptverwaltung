using System;

namespace RecipeLibrary.Models
{
    /// <summary>
    /// Represents an ingredient that can be used in recipes.  Ingredient names
    /// are globally unique.  Only the name is stored as there are no
    /// additional properties; if desired you could add units, nutritional
    /// information etc.
    /// </summary>
    public class Ingredient
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Unique name of the ingredient (e.g. "Flour").
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}