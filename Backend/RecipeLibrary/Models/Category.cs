using System;

namespace RecipeLibrary.Models
{
    /// <summary>
    /// Represents a category that can be used to group recipes, such as
    /// "Dessert", "Vegetarian" or "Main Course".  Category names must be
    /// unique across the system.
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The unique name of the category.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}