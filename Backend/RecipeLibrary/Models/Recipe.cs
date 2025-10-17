using System;
using System.Collections.Generic;

namespace RecipeLibrary.Models
{
    /// <summary>
    /// Represents a recipe owned by a user.  The recipe contains a globally
    /// unique name, a set of steps, a list of ingredient usages and a set of
    /// categories.  The OwnerId property links the recipe back to the user who
    /// created it.
    /// </summary>
    public class Recipe
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of the recipe.  Must be unique across all recipes.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The identifier of the user who owns this recipe.
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// List of preparation steps.  At least one step is required.
        /// </summary>
        public List<string> Steps { get; set; } = new List<string>();

        /// <summary>
        /// Ingredients used in the recipe along with quantities.
        /// At least one ingredient usage is required.
        /// </summary>
        public List<IngredientUsage> Ingredients { get; set; } = new List<IngredientUsage>();

        /// <summary>
        /// A collection of category identifiers associated with this recipe.
        /// At least one category is required.
        /// </summary>
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();
    }
}