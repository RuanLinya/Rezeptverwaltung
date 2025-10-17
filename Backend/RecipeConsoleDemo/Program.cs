using System;
using System.Collections.Generic;
using System.Linq;
using RecipeLibrary.Data;
using RecipeLibrary.Services;
using RecipeLibrary.Models;

namespace RecipeConsoleDemo
{
    /// <summary>
    /// The program supports registration, authentication and various CRUD operations.
    /// </summary>
    internal class Program
    {
        private static DataContext _context = new DataContext();
        private static UserService _userService = new UserService(_context);
        private static IngredientService _ingredientService = new IngredientService(_context);
        private static CategoryService _categoryService = new CategoryService(_context);
        private static RecipeService _recipeService = new RecipeService(_context, _ingredientService, _categoryService);
        private static FavouriteService _favouriteService = new FavouriteService(_context);

        public static void Main()
        {
            Console.WriteLine("Willkommen zum Rezeptverwaltungs‑Demo!");
            while (true)
            {
                try
                {
                    Console.WriteLine("\nHauptmenü");
                    Console.WriteLine("1) Benutzer registrieren");
                    Console.WriteLine("2) Anmelden");
                    Console.WriteLine("0) Beenden");
                    Console.Write("Auswahl: ");
                    var input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            RegisterUser();
                            break;
                        case "2":
                            var user = LoginUser();
                            if (user != null) UserMenu(user);
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Ungültige Auswahl.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler: {ex.Message}");
                }
            }
        }

        private static void RegisterUser()
        {
            Console.Write("Benutzername: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Passwort: ");
            var pw = ReadPassword();
            var user = _userService.Register(name, pw);
            Console.WriteLine($"Benutzer '{user.UserName}' erstellt. Bitte melden Sie sich jetzt an.");
        }

        private static User? LoginUser()
        {
            Console.Write("Benutzername: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Passwort: ");
            var pw = ReadPassword();
            var user = _userService.Authenticate(name, pw);
            if (user == null)
            {
                Console.WriteLine("Anmeldung fehlgeschlagen.");
                return null;
            }
            Console.WriteLine($"Willkommen, {user.UserName}!");
            return user;
        }

        private static void UserMenu(User user)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\nBenutzermenü");
                    Console.WriteLine("1) Kategorie anlegen");
                    Console.WriteLine("2) Zutat anlegen");
                    Console.WriteLine("3) Rezept anlegen");
                    Console.WriteLine("4) Eigene Rezepte anzeigen");
                    Console.WriteLine("5) Rezepte nach Kategorie anzeigen");
                    Console.WriteLine("6) Rezepte nach Zutat anzeigen");
                    Console.WriteLine("7) Rezept favorisieren");
                    Console.WriteLine("8) Favorisierung entfernen");
                    Console.WriteLine("9) Eigene Favoriten anzeigen");
                    Console.WriteLine("10) Rezept bearbeiten");
                    Console.WriteLine("11) Rezept löschen");
                    Console.WriteLine("12) Kategorie umbenennen");
                    Console.WriteLine("13) Kategorie löschen");
                    Console.WriteLine("14) Rezepte eines Nutzers anzeigen");
                    Console.WriteLine("15) Alle Zutaten anzeigen");
                    Console.WriteLine("0) Abmelden");
                    Console.Write("Auswahl: ");
                    var input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            CreateCategory();
                            break;
                        case "2":
                            CreateIngredient();
                            break;
                        case "3":
                            CreateRecipe(user);
                            break;
                        case "4":
                            ListOwnRecipes(user);
                            break;
                        case "5":
                            ListByCategory();
                            break;
                        case "6":
                            ListByIngredient();
                            break;
                        case "7":
                            FavouriteRecipe(user);
                            break;
                        case "8":
                            UnfavouriteRecipe(user);
                            break;
                        case "9":
                            ListFavourites(user);
                            break;
                        case "10":
                            EditRecipe(user);
                            break;
                        case "11":
                            DeleteRecipe(user);
                            break;
                        case "12":
                            RenameCategory();
                            break;
                        case "13":
                            DeleteCategory();
                            break;
                        case "14":
                            ListRecipesBySpecificUser();
                            break;
                        case "15":
                            ListAllIngredients();
                            break;
;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Ungültige Auswahl.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler: {ex.Message}");
                }
            }
        }

        private static void CreateCategory()
        {
            Console.Write("Neuer Kategoriename: ");
            var name = Console.ReadLine() ?? string.Empty;
            var cat = _categoryService.Create(name);
            Console.WriteLine($"Kategorie '{cat.Name}' erstellt.");
        }

        private static void CreateIngredient()
        {
            Console.Write("Name der Zutat: ");
            var name = Console.ReadLine() ?? string.Empty;
            var ing = _ingredientService.AddOrGet(name);
            Console.WriteLine($"Zutat '{ing.Name}' ist nun verfügbar.");
        }

        private static void CreateRecipe(User user)
        {
            Console.Write("Rezeptname: ");
            var recipeName = Console.ReadLine() ?? string.Empty;
            // Ingredients
            var ingredients = new List<(string, string)>();
            Console.WriteLine("Zutaten eingeben (leer lassen, um fertig zu sein). Format: Name|Menge");
            while (true)
            {
                Console.Write("Zutat|Menge: ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    break;
                var parts = line.Split('|');
                if (parts.Length != 2)
                {
                    Console.WriteLine("Bitte im Format 'Name|Menge' eingeben.");
                    continue;
                }
                ingredients.Add((parts[0].Trim(), parts[1].Trim()));
            }
            // Steps
            var steps = new List<string>();
            Console.WriteLine("Zubereitungsschritte eingeben (leer lassen, um fertig zu sein)");
            int stepIndex = 1;
            while (true)
            {
                Console.Write($"Schritt {stepIndex}: ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    break;
                steps.Add(line.Trim());
                stepIndex++;
            }
            // Categories
            var categories = new List<string>();
            Console.WriteLine("Kategorien eingeben (leer lassen, um fertig zu sein)");
            while (true)
            {
                Console.Write("Kategorie: ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    break;
                categories.Add(line.Trim());
            }
            var recipe = _recipeService.Create(user.Id, recipeName, ingredients, steps, categories);
            Console.WriteLine($"Rezept '{recipe.Name}' erstellt.");
        }

        private static void ListOwnRecipes(User user)
        {
            var recipes = _recipeService.GetByOwner(user.Id).ToList();
            if (!recipes.Any())
            {
                Console.WriteLine("Sie haben noch keine Rezepte.");
                return;
            }
            Console.WriteLine($"Rezepte von {user.UserName}:");
            foreach (var r in recipes)
            {
                Console.WriteLine($" - {r.Name} ({r.Id})");
            }
        }

        private static void ListByCategory()
        {
            Console.Write("Kategoriename: ");
            var catName = Console.ReadLine() ?? string.Empty;
            var category = _categoryService.GetByName(catName);
            if (category == null)
            {
                Console.WriteLine("Kategorie nicht gefunden.");
                return;
            }
            var recipes = _recipeService.GetByCategory(category.Id).ToList();
            if (!recipes.Any())
            {
                Console.WriteLine("Keine Rezepte in dieser Kategorie.");
                return;
            }
            Console.WriteLine($"Rezepte in Kategorie '{category.Name}':");
            foreach (var r in recipes)
            {
                Console.WriteLine($" - {r.Name} (Besitzer: {_userService.GetById(r.OwnerId)?.UserName ?? "Unbekannt"})");
            }
        }

        private static void ListByIngredient()
        {
            Console.Write("Zutatenname: ");
            var ingName = Console.ReadLine() ?? string.Empty;
            var ingredient = _ingredientService.GetByName(ingName);
            if (ingredient == null)
            {
                Console.WriteLine("Zutat nicht gefunden.");
                return;
            }
            var recipes = _recipeService.GetByIngredient(ingredient.Id).ToList();
            if (!recipes.Any())
            {
                Console.WriteLine("Keine Rezepte mit dieser Zutat.");
                return;
            }
            Console.WriteLine($"Rezepte mit '{ingredient.Name}':");
            foreach (var r in recipes)
            {
                Console.WriteLine($" - {r.Name} (Besitzer: {_userService.GetById(r.OwnerId)?.UserName ?? "Unbekannt"})");
            }
        }

        private static void FavouriteRecipe(User user)
        {
            // List all recipes from other users to choose from.
            var available = _context.Recipes
                .Where(r => r.OwnerId != user.Id)
                .ToList();
            if (!available.Any())
            {
                Console.WriteLine("Es gibt keine Rezepte anderer Benutzer, die favorisiert werden können.");
                return;
            }
            var selected = SelectItem(available, r => $"{r.Name} (von {_userService.GetById(r.OwnerId)?.UserName ?? "Unbekannt"})");
            if (selected == null) return;
            try
            {
                _favouriteService.AddFavourite(user.Id, selected.Id);
                Console.WriteLine($"Rezept '{selected.Name}' wurde als Favorit markiert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        private static void UnfavouriteRecipe(User user)
        {
            var favourites = _favouriteService.GetFavourites(user.Id).ToList();
            if (!favourites.Any())
            {
                Console.WriteLine("Sie haben keine favorisierten Rezepte.");
                return;
            }
            var selected = SelectItem(favourites, r => $"{r.Name} (von {_userService.GetById(r.OwnerId)?.UserName ?? "Unbekannt"})");
            if (selected == null) return;
            _favouriteService.RemoveFavourite(user.Id, selected.Id);
            Console.WriteLine($"Rezept '{selected.Name}' wurde aus den Favoriten entfernt.");
        }

        private static void ListFavourites(User user)
        {
            var favs = _favouriteService.GetFavourites(user.Id).ToList();
            if (!favs.Any())
            {
                Console.WriteLine("Sie haben keine favorisierten Rezepte.");
                return;
            }
            Console.WriteLine("Ihre Favoriten:");
            foreach (var r in favs)
            {
                Console.WriteLine($" - {r.Name} (Erstellt von: {_userService.GetById(r.OwnerId)?.UserName ?? "Unbekannt"})");
            }
        }

        /// <summary>
        /// Edits an existing recipe belonging to the current user.  Prompts for the recipe ID and new values.
        /// </summary>
        private static void EditRecipe(User user)
        {
            // Let the user select one of their own recipes to edit
            var ownRecipes = _recipeService.GetByOwner(user.Id).ToList();
            if (!ownRecipes.Any())
            {
                Console.WriteLine("Sie haben keine Rezepte zum Bearbeiten.");
                return;
            }
            var recipe = SelectItem(ownRecipes, r => r.Name);
            if (recipe == null) return;
            Console.Write($"Neuer Name ({recipe.Name}): ");
            var newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName))
            {
                // Keep old name if empty
                newName = recipe.Name;
            }
            // Ingredients
            Console.WriteLine("Neue Zutaten eingeben (leer lassen, um fertig zu sein). Format: Name|Menge");
            var newIngredients = new List<(string, string)>();
            while (true)
            {
                Console.Write("Zutat|Menge: ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;
                var parts = line.Split('|');
                if (parts.Length != 2)
                {
                    Console.WriteLine("Bitte im Format 'Name|Menge' eingeben.");
                    continue;
                }
                newIngredients.Add((parts[0].Trim(), parts[1].Trim()));
            }
            if (!newIngredients.Any())
            {
                // If the user didn't enter new ingredients keep the old ones by copying names and amounts
                newIngredients.AddRange(recipe.Ingredients.Select(i => (_ingredientService.GetById(i.IngredientId)?.Name ?? string.Empty, i.Amount)));
            }
            // Steps
            Console.WriteLine("Neue Schritte eingeben (leer lassen, um fertig zu sein)");
            var newSteps = new List<string>();
            int s = 1;
            while (true)
            {
                Console.Write($"Schritt {s}: ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;
                newSteps.Add(line.Trim());
                s++;
            }
            if (!newSteps.Any())
            {
                newSteps.AddRange(recipe.Steps);
            }
            // Categories
            Console.WriteLine("Neue Kategorien eingeben (leer lassen, um fertig zu sein)");
            var newCategories = new List<string>();
            while (true)
            {
                Console.Write("Kategorie: ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;
                newCategories.Add(line.Trim());
            }
            if (!newCategories.Any())
            {
                newCategories.AddRange(recipe.CategoryIds.Select(id => _categoryService.GetById(id)?.Name ?? string.Empty));
            }
            // Update the recipe using its current Id
            _recipeService.Update(recipe.Id, newName, newIngredients, newSteps, newCategories);
            Console.WriteLine("Rezept aktualisiert.");
        }

        /// <summary>
        /// Deletes a recipe belonging to the current user.
        /// </summary>
        private static void DeleteRecipe(User user)
        {
            // Select a recipe to delete
            var ownRecipes = _recipeService.GetByOwner(user.Id).ToList();
            if (!ownRecipes.Any())
            {
                Console.WriteLine("Sie haben keine Rezepte zum Löschen.");
                return;
            }
            var recipe = SelectItem(ownRecipes, r => r.Name);
            if (recipe == null) return;
            _recipeService.Delete(recipe.Id);
            Console.WriteLine($"Rezept '{recipe.Name}' gelöscht.");
        }

        /// <summary>
        /// Renames an existing category.  Prompts for the current name and the new name.
        /// </summary>
        private static void RenameCategory()
        {
            var categories = _categoryService.GetAll().ToList();
            if (!categories.Any())
            {
                Console.WriteLine("Es gibt keine Kategorien.");
                return;
            }
            var category = SelectItem(categories, c => c.Name);
            if (category == null) return;
            Console.Write($"Neuer Name für Kategorie '{category.Name}': ");
            var newName = Console.ReadLine() ?? string.Empty;
            _categoryService.Rename(category.Id, newName);
            Console.WriteLine("Kategorie umbenannt.");
        }

        /// <summary>
        /// Deletes a category if it is not used by any recipe.
        /// </summary>
        private static void DeleteCategory()
        {
            var categories = _categoryService.GetAll().ToList();
            if (!categories.Any())
            {
                Console.WriteLine("Es gibt keine Kategorien.");
                return;
            }
            var category = SelectItem(categories, c => c.Name);
            if (category == null) return;
            try
            {
                _categoryService.Delete(category.Id);
                Console.WriteLine($"Kategorie '{category.Name}' gelöscht.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Löschen: {ex.Message}");
            }
        }

        /// <summary>
        /// Presents the user with a numbered list of items and returns the selected item.
        /// If the user inputs 0 or an invalid number, null is returned.
        /// </summary>
        private static T? SelectItem<T>(IEnumerable<T> items, Func<T, string> display)
        {
            var list = items.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {display(list[i])}");
            }
            Console.Write("Auswahl (0 zum Abbrechen): ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int index) && index > 0 && index <= list.Count)
            {
                return list[index - 1];
            }
            Console.WriteLine("Abgebrochen oder ungültige Auswahl.");
            return default;
        }

        /// <summary>
        /// Reads a password from the console without echoing.  If the platform
        /// does not support masked input, falls back to plain read.
        /// </summary>
        private static string ReadPassword()
        {
            // Attempt to read the password character by character to mask input.
            // This can throw InvalidOperationException if the host does not have
            // an interactive console (e.g. when run via the VS Code Debug Console).
            try
            {
                var pwd = string.Empty;
                ConsoleKeyInfo key;
                do
                {
                    key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.Backspace && pwd.Length > 0)
                    {
                        pwd = pwd[..^1];
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        pwd += key.KeyChar;
                        Console.Write("*");
                    }
                } while (key.Key != ConsoleKey.Enter);
                Console.WriteLine();
                return pwd;
            }
            catch (InvalidOperationException)
            {
                // Fall back to unmasked password entry.  This happens if there
                // is no underlying console (e.g. Debug Console in VS Code).
                return Console.ReadLine() ?? string.Empty;
            }
        }
        private static void ListRecipesBySpecificUser()
        {
            Console.Write("Benutzername: ");
            var name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name darf nicht leer sein.");
                return;
            }

            var user = _userService.GetByName(name);
            if (user == null)
            {
                Console.WriteLine($"Benutzer '{name}' wurde nicht gefunden.");
                return;
            }

            var recipes = _recipeService.GetByOwner(user.Id).ToList();
            if (recipes.Count == 0)
            {
                Console.WriteLine($"Keine Rezepte von '{name}'.");
                return;
            }

            Console.WriteLine($"Rezepte von '{name}':");
            foreach (var r in recipes)
                Console.WriteLine($"- {r.Name} ({r.Id})");
        }

        private static void ListAllIngredients()
        {
            var ings = _ingredientService.GetAll().OrderBy(i => i.Name).ToList();
            if (ings.Count == 0)
            {
                Console.WriteLine("Keine Zutaten vorhanden.");
                return;
            }
            Console.WriteLine("Globale Zutatenliste:");
            foreach (var ing in ings)
                Console.WriteLine($"- {ing.Name} ({ing.Id})");
        }

    }
}