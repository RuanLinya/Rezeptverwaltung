using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using RecipeLibrary.Models;

namespace RecipeLibrary.Data
{
    /// <summary>
    /// DataContext manages the persistent storage of all domain entities.  It
    /// serialises collections of users, recipes, categories and ingredients to
    /// JSON files on disk.  When instantiated it attempts to load existing
    /// data from disk, and on save writes the current state back to disk.
    /// </summary>
    public class DataContext
    {
        private readonly string _directory;

        public List<User> Users { get; private set; } = new List<User>();
        public List<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();
        public List<Category> Categories { get; private set; } = new List<Category>();
        public List<Recipe> Recipes { get; private set; } = new List<Recipe>();

        /// <summary>
        /// The default constructor uses a directory named "data" relative to
        /// the application's working directory.
        /// </summary>
        public DataContext() : this(Path.Combine(Environment.CurrentDirectory, "data"))
        {
        }

        /// <summary>
        /// Instantiates a new DataContext using the given directory.  The
        /// directory is created if it does not exist.  All JSON files are
        /// loaded if present; otherwise empty collections are initialised.
        /// </summary>
        /// <param name="directory">Directory where the JSON files are stored.</param>
        public DataContext(string directory)
        {
            _directory = directory;
            Directory.CreateDirectory(_directory);
            LoadAll();
        }

        private void LoadAll()
        {
            Users = Load<List<User>>(Path.Combine(_directory, "users.json")) ?? new List<User>();
            Ingredients = Load<List<Ingredient>>(Path.Combine(_directory, "ingredients.json")) ?? new List<Ingredient>();
            Categories = Load<List<Category>>(Path.Combine(_directory, "categories.json")) ?? new List<Category>();
            Recipes = Load<List<Recipe>>(Path.Combine(_directory, "recipes.json")) ?? new List<Recipe>();
        }

        /// <summary>
        /// Generic method to load an object from JSON file.  Returns null if
        /// the file does not exist or cannot be deserialised.
        /// </summary>
        private static T? Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                });
            }
            catch
            {
                // If deserialisation fails we ignore the file.  In a real system
                // you might want to log this error or rethrow.
                return default;
            }
        }

        /// <summary>
        /// Saves all collections to their respective JSON files.  The entire
        /// contents are overwritten on each call.
        /// </summary>
        public void SaveChanges()
        {
            Save(Path.Combine(_directory, "users.json"), Users);
            Save(Path.Combine(_directory, "ingredients.json"), Ingredients);
            Save(Path.Combine(_directory, "categories.json"), Categories);
            Save(Path.Combine(_directory, "recipes.json"), Recipes);
        }

        private static void Save(string filePath, object data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            });
            File.WriteAllText(filePath, json);
        }
    }
}