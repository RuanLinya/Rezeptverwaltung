using System;
using System.Linq;
using System.Collections.Generic;
using RecipeLibrary.Data;
using RecipeLibrary.Models;

namespace RecipeLibrary.Services
{
    /// <summary>
    /// Manages a user's favourite recipes.  Favourites are stored as a set of
    /// recipe identifiers on the User entity.  Only recipes belonging to
    /// other users can be marked as favourite.
    /// </summary>
    public class FavouriteService
    {
        private readonly DataContext _context;

        public FavouriteService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Marks a recipe as favourite for the given user.  Throws if the
        /// recipe does not exist, if the user does not exist or if the user
        /// owns the recipe (you cannot favourite your own recipe).
        /// </summary>
        public void AddFavourite(Guid userId, Guid recipeId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId) ?? throw new KeyNotFoundException("User not found.");
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == recipeId) ?? throw new KeyNotFoundException("Recipe not found.");
            if (recipe.OwnerId == userId)
                throw new InvalidOperationException("Users cannot favourite their own recipes.");
            user.FavouriteRecipeIds.Add(recipeId);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes a recipe from the user's favourites.  Throws if the user
        /// does not exist.  Silently does nothing if the recipe is not a
        /// favourite.
        /// </summary>
        public void RemoveFavourite(Guid userId, Guid recipeId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId) ?? throw new KeyNotFoundException("User not found.");
            user.FavouriteRecipeIds.Remove(recipeId);
            _context.SaveChanges();
        }

        /// <summary>
        /// Returns all recipes that the user has marked as favourite.
        /// </summary>
        public IEnumerable<Recipe> GetFavourites(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId) ?? throw new KeyNotFoundException("User not found.");
            return _context.Recipes.Where(r => user.FavouriteRecipeIds.Contains(r.Id));
        }
    }
}