using System;

namespace RecipeLibrary.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Create a new unique ID. Data Layer

        public string Name { get; set; } = string.Empty;
    }
}