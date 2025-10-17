using System;

namespace RecipeLibrary.Models
{
    /// <summary>
    /// Represents the usage of an ingredient in a particular recipe.  It
    /// consists of a reference to the ingredient and a textual description of
    /// the amount and unit (e.g. "2 cups", "200 g").  You could extend this
    /// class with numeric properties if you wanted to perform unit
    /// conversions.
    /// </summary>
    public class IngredientUsage
    {
        public Guid IngredientId { get; set; }
        public string Amount { get; set; } = string.Empty;
    }
}