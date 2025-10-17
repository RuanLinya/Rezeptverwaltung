using System;
using System.Collections.Generic;
using System.Linq;
using RecipeLibrary.Data;
using RecipeLibrary.Models;

namespace RecipeLibrary.Services
{
    public class RecipeService
    {
        private readonly DataContext _context;
        private readonly IngredientService _ingredientService;
        private readonly CategoryService _categoryService;

        public RecipeService(DataContext context, IngredientService ingredientService, CategoryService categoryService)
        {
            _context = context;
            _ingredientService = ingredientService;
            _categoryService = categoryService;
        }

        /// </summary>
        /// <param name="ownerId">Id of the user creating the recipe.</param>
        /// <param name="name">Unique name of the recipe.</param>
        /// <param name="ingredients">Collection of tuples containing the ingredient name and the amount/description.</param>
        /// <param name="steps">List of preparation steps.</param>
        /// <param name="categories">List of category names.</param>
        public Recipe Create(Guid ownerId,
                             string name,
                             IEnumerable<(string ingredientName, string amount)> ingredients,
                             IEnumerable<string> steps,
                             IEnumerable<string> categories)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Recipe name must not be empty.", nameof(name));
            if (_context.Recipes.Any(r => string.Equals(r.Name, name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"A recipe named '{name}' already exists.");
            var ingredientList = ingredients?.ToList() ?? throw new ArgumentNullException(nameof(ingredients));
            if (!ingredientList.Any())
                throw new InvalidOperationException("A recipe must contain at least one ingredient.");
            var stepList = steps?.ToList() ?? throw new ArgumentNullException(nameof(steps));
            if (!stepList.Any())
                throw new InvalidOperationException("A recipe must contain at least one preparation step.");
            var categoryList = categories?.ToList() ?? throw new ArgumentNullException(nameof(categories));
            if (!categoryList.Any())
                throw new InvalidOperationException("A recipe must belong to at least one category.");

            // Resolve or create ingredients.
            var ingredientUsages = new List<IngredientUsage>();
            foreach (var (ingredientName, amount) in ingredientList)
            {
                if (string.IsNullOrWhiteSpace(ingredientName))
                    throw new ArgumentException("Ingredient name must not be empty.");
                if (string.IsNullOrWhiteSpace(amount))
                    throw new ArgumentException("Ingredient amount must not be empty.");
                var ingredient = _ingredientService.AddOrGet(ingredientName);
                ingredientUsages.Add(new IngredientUsage
                {
                    IngredientId = ingredient.Id,
                    Amount = amount
                });
            }

            // Resolve or create categories.
            var categoryIds = new List<Guid>();
            foreach (var categoryName in categoryList)
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("Category name must not be empty.");
                var category = _categoryService.GetByName(categoryName);
                if (category == null)
                {
                    category = _categoryService.Create(categoryName);
                }
                categoryIds.Add(category.Id);
            }

            var recipe = new Recipe
            {
                Id = Guid.NewGuid(),
                Name = name,
                OwnerId = ownerId,
                Ingredients = ingredientUsages,
                Steps = stepList,
                CategoryIds = categoryIds
            };
            _context.Recipes.Add(recipe);
            _context.SaveChanges();
            return recipe;
        }
        public void Update(Guid recipeId,
                           string newName,
                           IEnumerable<(string ingredientName, string amount)> ingredients,
                           IEnumerable<string> steps,
                           IEnumerable<string> categories)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == recipeId) ?? throw new KeyNotFoundException("Recipe not found.");
            if (!string.Equals(recipe.Name, newName, StringComparison.OrdinalIgnoreCase))
            {
                // Name changed; ensure uniqueness.
                if (_context.Recipes.Any(r => r.Id != recipeId && string.Equals(r.Name, newName, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException($"A recipe named '{newName}' already exists.");
            }
            // Validate lists similarly to Create.
            var ingredientList = ingredients?.ToList() ?? throw new ArgumentNullException(nameof(ingredients));
            if (!ingredientList.Any())
                throw new InvalidOperationException("A recipe must contain at least one ingredient.");
            var stepList = steps?.ToList() ?? throw new ArgumentNullException(nameof(steps));
            if (!stepList.Any())
                throw new InvalidOperationException("A recipe must contain at least one preparation step.");
            var categoryList = categories?.ToList() ?? throw new ArgumentNullException(nameof(categories));
            if (!categoryList.Any())
                throw new InvalidOperationException("A recipe must belong to at least one category.");

            // Resolve or create ingredients.
            var ingredientUsages = new List<IngredientUsage>();
            foreach (var (ingredientName, amount) in ingredientList)
            {
                if (string.IsNullOrWhiteSpace(ingredientName))
                    throw new ArgumentException("Ingredient name must not be empty.");
                if (string.IsNullOrWhiteSpace(amount))
                    throw new ArgumentException("Ingredient amount must not be empty.");
                var ingredient = _ingredientService.AddOrGet(ingredientName);
                ingredientUsages.Add(new IngredientUsage { IngredientId = ingredient.Id, Amount = amount });
            }
            // Resolve or create categories.
            var categoryIds = new List<Guid>();
            foreach (var categoryName in categoryList)
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("Category name must not be empty.");
                var category = _categoryService.GetByName(categoryName);
                if (category == null)
                {
                    category = _categoryService.Create(categoryName);
                }
                categoryIds.Add(category.Id);
            }
            recipe.Name = newName;
            recipe.Ingredients = ingredientUsages;
            recipe.Steps = stepList;
            recipe.CategoryIds = categoryIds;
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a recipe and removes it from all users' favourites.
        /// </summary>
        public void Delete(Guid recipeId)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == recipeId) ?? throw new KeyNotFoundException("Recipe not found.");
            _context.Recipes.Remove(recipe);
            // Remove from favourites of all users.
            foreach (var user in _context.Users)
            {
                user.FavouriteRecipeIds.Remove(recipeId);
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Returns all recipes created by the specified user.
        /// </summary>
        public IEnumerable<Recipe> GetByOwner(Guid ownerId)
        {
            return _context.Recipes.Where(r => r.OwnerId == ownerId);
        }

        /// <summary>
        /// Returns all recipes associated with the given category identifier.
        /// </summary>
        public IEnumerable<Recipe> GetByCategory(Guid categoryId)
        {
            return _context.Recipes.Where(r => r.CategoryIds.Contains(categoryId));
        }

        /// <summary>
        /// Returns all recipes that include the specified ingredient identifier.
        /// </summary>
        public IEnumerable<Recipe> GetByIngredient(Guid ingredientId)
        {
            return _context.Recipes.Where(r => r.Ingredients.Any(i => i.IngredientId == ingredientId));
        }

        /// <summary>
        /// Retrieves a recipe by its identifier.
        /// </summary>
        public Recipe? GetById(Guid id)
        {
            return _context.Recipes.FirstOrDefault(r => r.Id == id);
        }
        public IEnumerable<Recipe> GetAll()
        {
            return _context.Recipes;
        }
    }
}