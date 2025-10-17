using System;
using System.Linq;
using System.Collections.Generic;
using RecipeLibrary.Data;
using RecipeLibrary.Models;

namespace RecipeLibrary.Services
{
    /// <summary>
    /// Provides CRUD operations for categories.  Category names must be unique.
    /// A category cannot be deleted if it is referenced by any recipe.
    /// </summary>
    public class CategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new category with the given name.  Throws if the name is
        /// empty or if a category with the same name already exists.
        /// </summary>
        public Category Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name must not be empty.", nameof(name));
            if (_context.Categories.Any(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Category '{name}' already exists.");

            var cat = new Category { Id = Guid.NewGuid(), Name = name };
            _context.Categories.Add(cat);
            _context.SaveChanges();
            return cat;
        }

        /// <summary>
        /// Renames the category with the specified identifier.  Throws if
        /// the category does not exist or if the new name is already in use by
        /// another category.
        /// </summary>
        public void Rename(Guid categoryId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("New name must not be empty.", nameof(newName));
            var cat = _context.Categories.FirstOrDefault(c => c.Id == categoryId) ?? throw new KeyNotFoundException("Category not found.");
            if (_context.Categories.Any(c => c.Id != categoryId && string.Equals(c.Name, newName, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"A category named '{newName}' already exists.");
            cat.Name = newName;
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes the category with the specified identifier.  Throws if the
        /// category is used by any recipe or does not exist.
        /// </summary>
        public void Delete(Guid categoryId)
        {
            var cat = _context.Categories.FirstOrDefault(c => c.Id == categoryId) ?? throw new KeyNotFoundException("Category not found.");
            // Ensure no recipe references this category.
            if (_context.Recipes.Any(r => r.CategoryIds.Contains(categoryId)))
            {
                throw new InvalidOperationException("Cannot delete category because it is assigned to one or more recipes.");
            }
            _context.Categories.Remove(cat);
            _context.SaveChanges();
        }

        /// <summary>
        /// Retrieves a category by its identifier.
        /// </summary>
        public Category? GetById(Guid id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Retrieves a category by name (case insensitive).
        /// </summary>
        public Category? GetByName(string name)
        {
            return _context.Categories.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns all categories sorted by name.
        /// </summary>
        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.OrderBy(c => c.Name);
        }
    }
}