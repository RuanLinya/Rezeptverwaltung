using System;
using System.Collections.Generic;
using System.Linq;
using RecipeLibrary.Data;
using RecipeLibrary.Models;
using RecipeLibrary.Services;

namespace RecipeConsoleDemo
{
    /// <summary>
    /// Automated end-to-end testing:
    /// Simulates the complete user workflow (registration → login → categories/ingredients/recipes → favourites → edit/delete)
    /// outputs a comprehensive visual summary at the conclusion.
    /// </summary>
    internal class AutoTestProgram
    {
        private static DataContext _context = new DataContext();
        private static UserService _userService = new UserService(_context);
        private static IngredientService _ingredientService = new IngredientService(_context);
        private static CategoryService _categoryService = new CategoryService(_context);
        private static RecipeService _recipeService = new RecipeService(_context, _ingredientService, _categoryService);
        private static FavouriteService _favouriteService = new FavouriteService(_context);

        public static void Run()
        {
            Console.Clear();
            PrintHeader("Automatisierte Funktionstests gestartet");

            
            // Clear old data to prevent duplicate username errors
            _context.Users.Clear();
            _context.Categories.Clear();
            _context.Ingredients.Clear();
            _context.Recipes.Clear();
            _context.SaveChanges();

            // Hauptmenü Simulated Registration and Login
            var user1 = _userService.Register("user1", "pw1");
            var user2 = _userService.Register("user2", "pw2");
            var login = _userService.Authenticate("user1", "pw1");
            Success($"Angemeldet als {login.UserName}");

            // UserMenu Functional Testing
            CreateCategories();
            CreateIngredients();
            CreateRecipes(login);
            ListOwnRecipes(login);
            FavouriteSystem(login, user2);
            EditAndDeleteRecipe(login);
            CategoryRenameAndDelete();
            VisualizeFinalSummary();
        }

        // Categories
        private static void CreateCategories()
        {
            PrintHeader("Kategorie-Tests");
            _categoryService.Create("Dessert");
            _categoryService.Create("Vegan");
            _categoryService.Create("Fastfood");
            Success("3 Kategorien erstellt.");
        }

       private static void CategoryRenameAndDelete()
        {
            PrintHeader("Kategorie-Tests: Umbenennen & Löschen");

            var all = _categoryService.GetAll().ToList();
            if (!all.Any())
            {
                Error("Keine Kategorien vorhanden.");
                return;
            }

            // Rename
            var first = all.First();
            _categoryService.Rename(first.Id, first.Name + " (neu)");
            Success($"Kategorie '{first.Name}' umbenannt zu '{first.Name} (neu)'.");

            // Secure deletion — only delete if category is not in use
            var allRecipes = _recipeService.GetAll().ToList();
            var unused = all.Where(cat => !allRecipes.Any(r => r.CategoryIds.Contains(cat.Id))).ToList();

            if (unused.Count > 0)
            {
                var del = unused.First();
                _categoryService.Delete(del.Id);
                Success($"Kategorie '{del.Name}' wurde gelöscht (nicht verwendet).");
            }
            else
            {
                Success("Keine unbenutzten Kategorien zum Löschen gefunden – übersprungen.");
            }
        }

        // Ingredients 
        private static void CreateIngredients()
        {
            PrintHeader("Zutaten-Tests");
            string[] names = { "Zucker", "Mehl", "Milch", "Eier", "Salz" };
            foreach (var n in names)
                _ingredientService.AddOrGet(n);
            Success($"{names.Length} Zutaten hinzugefügt.");
        }

        // recipes
        private static void CreateRecipes(User user)
        {
            PrintHeader("Rezept-Tests");

            var ingredients = new List<(string, string)> {
                ("Mehl", "200g"), ("Milch", "250ml"), ("Eier", "2 Stück")
            };
            var steps = new List<string> { "Alles verrühren", "In Pfanne goldbraun backen" };
            var categories = new List<string> { "Dessert" };

            var recipe = _recipeService.Create(user.Id, "Pfannkuchen", ingredients, steps, categories);
            Success($"Rezept '{recipe.Name}' erstellt.");

            // Zweites Rezept
            var ingredients2 = new List<(string, string)> { ("Salz", "1 TL"), ("Zucker", "2 TL") };
            _recipeService.Create(user.Id, "Toastbrot", ingredients2, new List<string> { "Backen bei 180°C" }, new List<string> { "Vegan" });
            Success("2. Rezept 'Toastbrot' erstellt.");
        }

        private static void ListOwnRecipes(User user)
        {
            PrintHeader($"Eigene Rezepte von {user.UserName}");
            var recipes = _recipeService.GetByOwner(user.Id).ToList();
            PrintTable(recipes, r => r.Name, r => string.Join(", ", r.CategoryIds.Select(id => _categoryService.GetById(id)?.Name ?? "")));
        }

       private static void EditAndDeleteRecipe(User user)
        {
            PrintHeader("Rezept-Bearbeitung");

            var recipe = _recipeService.GetByOwner(user.Id).FirstOrDefault();
            if (recipe == null)
            {
                Error("Keine Rezepte zum Bearbeiten.");
                return;
            }

            // Retrieve ingredient names and quantities 
            var ingredients = new List<(string, string)>();
            foreach (var ri in _ingredientService.GetAll())
            {
                // Simulate providing a fixed quantity to prevent triggering the ‘at least one ingredient’ exception during updates.
                ingredients.Add((ri.Name, "1 Stück"));
            }

            // Obtain steps and classifications
            var steps = new List<string> { "Zutaten prüfen", "Schmeckt gut servieren" };
            var categories = _categoryService.GetAll().Select(c => c.Name).ToList();

            _recipeService.Update(recipe.Id,
                recipe.Name + " (neu)",
                ingredients,
                steps,
                categories);

            Success($"Rezept '{recipe.Name}' umbenannt zu '{recipe.Name} (neu)'.");

            // Then safely remove another recipe
            var all = _recipeService.GetByOwner(user.Id).ToList();
            if (all.Count > 1)
            {
                var toDelete = all.Last();
                _recipeService.Delete(toDelete.Id);
                Success($"Rezept '{toDelete.Name}' gelöscht.");
            }
        }


        //  favourites 
        private static void FavouriteSystem(User user, User secondUser)
        {
            PrintHeader("Favoriten-Tests");

            var recipe = _recipeService.Create(
                secondUser.Id,
                "Pizza",
                new List<(string, string)> { ("Mehl", "300g"), ("Zucker", "1 TL") },
                new List<string> { "Kneten", "Backen" },
                new List<string> { "Fastfood" }
            );

            _favouriteService.AddFavourite(user.Id, recipe.Id);
            Success($"'{recipe.Name}' favorisiert.");

            var favs = _favouriteService.GetFavourites(user.Id).ToList();
            PrintTable(favs, r => r.Name, r => _userService.GetById(r.OwnerId)?.UserName ?? "?");

            _favouriteService.RemoveFavourite(user.Id, recipe.Id);
            Success($"'{recipe.Name}' wieder entfernt.");
        }

        // Visualisation output
        private static void VisualizeFinalSummary()
        {
            PrintHeader("Automatische Zusammenfassung");

            Console.WriteLine("\n Benutzer:");
            PrintTable(_context.Users, u => u.UserName, u => u.Id);

            Console.WriteLine("\n Kategorien:");
            PrintTable(_context.Categories, c => c.Name, c => c.Id);

            Console.WriteLine("\n Zutaten:");
            PrintTable(_context.Ingredients, i => i.Name, i => i.Id);

            Console.WriteLine("\n Rezepte:");
            PrintTable(_context.Recipes,
                r => r.Name,
                r => _userService.GetById(r.OwnerId)?.UserName ?? "Unbekannt",
                r => string.Join(", ", r.CategoryIds.Select(id => _categoryService.GetById(id)?.Name ?? ""))
            );

            Console.WriteLine("\n Favoriten:");
            var favs = _context.Users.SelectMany(u =>
                _favouriteService.GetFavourites(u.Id)
                    .Select(f => new { User = u.UserName, Recipe = f.Name })
            ).ToList();

            if (favs.Count > 0)
                PrintTable(favs, f => f.User, f => f.Recipe);
            else
                Console.WriteLine("(Keine Favoriten)");

            Console.WriteLine("\n══════════════════════════════════════════════");
            Success("Automatisierte Tests erfolgreich abgeschlossen!");
            Console.WriteLine("══════════════════════════════════════════════");
        }

        // Visualisation output tool
        private static void PrintHeader(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n" + new string('═', text.Length + 4));
            Console.WriteLine($"  {text}");
            Console.WriteLine(new string('═', text.Length + 4));
            Console.ResetColor();
        }

        private static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✔ " + message);
            Console.ResetColor();
        }

        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("✖ " + message);
            Console.ResetColor();
        }

        private static void PrintTable<T>(IEnumerable<T> items, params Func<T, object>[] columns)
        {
            var list = items.ToList();
            if (!list.Any())
            {
                Console.WriteLine("(keine Daten)");
                return;
            }

            var rows = list.Select(item => columns.Select(c => c(item)?.ToString() ?? "").ToList()).ToList();
            var colWidths = Enumerable.Range(0, columns.Length)
                .Select(i => rows.Max(r => r[i].Length)).ToList();

            Console.WriteLine("┌" + string.Join("┬", colWidths.Select(w => new string('─', w + 2))) + "┐");
            foreach (var row in rows)
            {
                for (int i = 0; i < row.Count; i++)
                    Console.Write("│ " + row[i].PadRight(colWidths[i]) + " ");
                Console.WriteLine("│");
            }
            Console.WriteLine("└" + string.Join("┴", colWidths.Select(w => new string('─', w + 2))) + "┘");
        }
    }
}
