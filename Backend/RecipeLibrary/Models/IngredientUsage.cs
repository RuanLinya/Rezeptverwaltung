using System;

namespace RecipeLibrary.Models
{
    public class IngredientUsage
    {
        public Guid IngredientId { get; set; }
        public string Amount { get; set; } = string.Empty;
    }
}