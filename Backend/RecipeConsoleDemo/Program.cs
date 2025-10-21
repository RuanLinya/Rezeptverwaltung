using System;
using System.Collections.Generic;
using System.Linq;
using RecipeLibrary.Data;
using RecipeLibrary.Services;
using RecipeLibrary.Models;

namespace RecipeConsoleDemo
{
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
                    Console.OutputEncoding = System.Text.Encoding.UTF8;
                    Console.WriteLine("═══════════════════════════════════════════════");
                    Console.WriteLine(" Willkommen zum Rezeptverwaltungs-Demo!");
                    Console.WriteLine("═══════════════════════════════════════════════");
                    Console.Write("Automatischer Testmodus starten? (J/N): ");
                    var key = Console.ReadLine()?.Trim().ToLower();

                    // Automatic Mode Detection
                    if (key == "j")
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Starte automatischen Testmodus...\n");
                        Console.ResetColor();

                        // Invoke the automated testing entry point
                        AutoTestProgram.Run();
                        return;
                    }

                    // Otherwise, proceed to the original interactive console
                    Console.WriteLine("\nStarte interaktives Hauptmenü...\n");
                    RunInteractiveMenu();
                }

                private static void RunInteractiveMenu()
                {
                    while (true)
                    {
                        try
                        {
                            PrintHeader("Hauptmenü");
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
                                    Console.WriteLine("Auf Wiedersehen!");
                                    return;
                                default:
                                    Error("Ungültige Auswahl.");
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Error($"Fehler: {ex.Message}");
                        }
                    }
                }
        // User management  
        private static void RegisterUser()
        {
            Console.Write("Benutzername: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Passwort: ");
            var pw = ReadPassword();
            var user = _userService.Register(name, pw);
            Success($"Benutzer '{user.UserName}' erstellt. Bitte melden Sie sich jetzt an.");
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
                Error("Anmeldung fehlgeschlagen.");
                return null;
            }
            Success($"Willkommen, {user.UserName}!");
            return user;
        }

        // User menu  
        private static void UserMenu(User user)
        {
            while (true)
            {
                try
                {
                    PrintHeader($"Menü für {user.UserName}");
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
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Error("Keine Eingabe erkannt. Bitte Zahl eingeben und Enter drücken.");
                        continue;
                    }

                    switch (input.Trim())
                    {
                        case "1": CreateCategory(); break;
                        case "2": CreateIngredient(); break;
                        case "3": CreateRecipe(user); break;
                        case "4": ListOwnRecipes(user); break;
                        case "5": ListByCategory(); break;
                        case "6": ListByIngredient(); break;
                        case "7": FavouriteRecipe(user); break;
                        case "8": UnfavouriteRecipe(user); break;
                        case "9": ListFavourites(user); break;
                        case "10": EditRecipe(user); break;
                        case "11": DeleteRecipe(user); break;
                        case "12": RenameCategory(); break;
                        case "13": DeleteCategory(); break;
                        case "14": ListRecipesBySpecificUser(); break;
                        case "15": ListAllIngredients(); break;
                        case "0": return;
                        default: Error("Ungültige Auswahl."); break;
                    }
                }
                catch (Exception ex)
                {
                    Error($"Fehler: {ex.Message}");
                }
            }
        }

        // Categories / Ingredients
        private static void CreateCategory()
        {
            Console.Write("Neuer Kategoriename: ");
            var name = Console.ReadLine() ?? string.Empty;
            var cat = _categoryService.Create(name);
            Success($"Kategorie '{cat.Name}' erstellt.");
        }

        private static void CreateIngredient()
        {
            Console.Write("Name der Zutat: ");
            var name = Console.ReadLine() ?? string.Empty;
            var ing = _ingredientService.AddOrGet(name);
            Success($"Zutat '{ing.Name}' ist nun verfügbar.");
        }

        // Recipes  
        private static void CreateRecipe(User user)
        {
            Console.Write("Rezeptname: ");
            var recipeName = Console.ReadLine() ?? string.Empty;

            // Zutaten
            var ingredients = new List<(string, string)>();
            Console.WriteLine("Zutaten eingeben (leer lassen, um fertig zu sein). Format: Name|Menge");
            while (true)
            {
                Console.Write("Zutat|Menge: ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;
                var parts = line.Split('|');
                if (parts.Length != 2)
                {
                    Error("Bitte im Format 'Name|Menge' eingeben.");
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
                if (string.IsNullOrWhiteSpace(line)) break;
                steps.Add(line.Trim());
                stepIndex++;
            }

            // Kategorien
            var categories = new List<string>();
            Console.WriteLine("Kategorien eingeben (leer lassen, um fertig zu sein)");
            while (true)
            {
                Console.Write("Kategorie: ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;
                categories.Add(line.Trim());
            }

            var recipe = _recipeService.Create(user.Id, recipeName, ingredients, steps, categories);
            Success($"Rezept '{recipe.Name}' erstellt.");
        }

        private static void EditRecipe(User user)
        {
            var ownRecipes = _recipeService.GetByOwner(user.Id).ToList();
            if (!ownRecipes.Any())
            {
                Error("Keine Rezepte zum Bearbeiten.");
                return;
            }
            var recipe = SelectItem(ownRecipes, r => r.Name);
            if (recipe == null) return;

            Console.Write($"Neuer Name ({recipe.Name}): ");
            var newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName)) newName = recipe.Name;

            _recipeService.Update(recipe.Id, newName,
                new List<(string, string)>(),
                new List<string>(),
                new List<string>());

            Success("Rezept aktualisiert.");
        }

        private static void DeleteRecipe(User user)
        {
            var ownRecipes = _recipeService.GetByOwner(user.Id).ToList();
            if (!ownRecipes.Any())
            {
                Error("Keine Rezepte zum Löschen.");
                return;
            }
            var recipe = SelectItem(ownRecipes, r => r.Name);
            if (recipe == null) return;
            _recipeService.Delete(recipe.Id);
            Success($"Rezept '{recipe.Name}' gelöscht.");
        }

        //  Listen / Favourites
        private static void ListOwnRecipes(User user)
        {
            var recipes = _recipeService.GetByOwner(user.Id).ToList();
            if (!recipes.Any())
            {
                Error("Sie haben noch keine Rezepte.");
                return;
            }
            PrintHeader($"Rezepte von {user.UserName}");
            PrintTable(recipes,
                r => r.Name,
                r => string.Join(", ", r.CategoryIds.Select(id => _categoryService.GetById(id)?.Name ?? ""))
            );
        }

        private static void ListByCategory()
        {
            Console.Write("Kategoriename: ");
            var catName = Console.ReadLine() ?? string.Empty;
            var category = _categoryService.GetByName(catName);
            if (category == null)
            {
                Error("Kategorie nicht gefunden.");
                return;
            }
            var recipes = _recipeService.GetByCategory(category.Id).ToList();
            if (!recipes.Any())
            {
                Error("Keine Rezepte in dieser Kategorie.");
                return;
            }
            PrintHeader($"Rezepte in '{category.Name}'");
            PrintTable(recipes,
                r => r.Name,
                r => _userService.GetById(r.OwnerId)?.UserName ?? "Unbekannt"
            );
        }

        private static void ListByIngredient()
        {
            Console.Write("Zutatenname: ");
            var ingName = Console.ReadLine() ?? string.Empty;
            var ingredient = _ingredientService.GetByName(ingName);
            if (ingredient == null)
            {
                Error("Zutat nicht gefunden.");
                return;
            }
            var recipes = _recipeService.GetByIngredient(ingredient.Id).ToList();
            if (!recipes.Any())
            {
                Error("Keine Rezepte mit dieser Zutat.");
                return;
            }
            PrintHeader($"Rezepte mit '{ingredient.Name}'");
            PrintTable(recipes,
                r => r.Name,
                r => _userService.GetById(r.OwnerId)?.UserName ?? "Unbekannt"
            );
        }

        private static void FavouriteRecipe(User user)
        {
            var available = _context.Recipes.Where(r => r.OwnerId != user.Id).ToList();
            if (!available.Any())
            {
                Error("Keine fremden Rezepte verfügbar.");
                return;
            }
            var selected = SelectItem(available, r => $"{r.Name} (von {_userService.GetById(r.OwnerId)?.UserName ?? "?"})");
            if (selected == null) return;
            _favouriteService.AddFavourite(user.Id, selected.Id);
            Success($"Rezept '{selected.Name}' favorisiert.");
        }

        private static void UnfavouriteRecipe(User user)
        {
            var favourites = _favouriteService.GetFavourites(user.Id).ToList();
            if (!favourites.Any())
            {
                Error("Keine Favoriten.");
                return;
            }
            var selected = SelectItem(favourites, r => r.Name);
            if (selected == null) return;
            _favouriteService.RemoveFavourite(user.Id, selected.Id);
            Success($"Rezept '{selected.Name}' entfernt.");
        }

        private static void ListFavourites(User user)
        {
            var favs = _favouriteService.GetFavourites(user.Id).ToList();
            if (!favs.Any())
            {
                Error("Keine Favoriten gefunden.");
                return;
            }
            PrintHeader("Ihre Favoriten");
            PrintTable(favs,
                r => r.Name,
                r => _userService.GetById(r.OwnerId)?.UserName ?? "?"
            );
        }

        // Help functions  
        private static void RenameCategory()
        {
            var categories = _categoryService.GetAll().ToList();
            if (!categories.Any())
            {
                Error("Keine Kategorien.");
                return;
            }
            var category = SelectItem(categories, c => c.Name);
            if (category == null) return;
            Console.Write($"Neuer Name für '{category.Name}': ");
            var newName = Console.ReadLine() ?? string.Empty;
            _categoryService.Rename(category.Id, newName);
            Success("Kategorie umbenannt.");
        }

        private static void DeleteCategory()
        {
            var categories = _categoryService.GetAll().ToList();
            if (!categories.Any())
            {
                Error("Keine Kategorien.");
                return;
            }
            var category = SelectItem(categories, c => c.Name);
            if (category == null) return;
            _categoryService.Delete(category.Id);
            Success($"Kategorie '{category.Name}' gelöscht.");
        }

        private static void ListRecipesBySpecificUser()
        {
            Console.Write("Benutzername: ");
            var name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                Error("Name darf nicht leer sein.");
                return;
            }
            var user = _userService.GetByName(name);
            if (user == null)
            {
                Error($"Benutzer '{name}' nicht gefunden.");
                return;
            }
            var recipes = _recipeService.GetByOwner(user.Id).ToList();
            if (!recipes.Any())
            {
                Error($"Keine Rezepte von '{name}'.");
                return;
            }
            PrintHeader($"Rezepte von '{name}'");
            PrintTable(recipes, r => r.Name, r => r.Id);
        }

        private static void ListAllIngredients()
        {
            var ings = _ingredientService.GetAll().OrderBy(i => i.Name).ToList();
            if (ings.Count == 0)
            {
                Error("Keine Zutaten vorhanden.");
                return;
            }
            PrintHeader("Globale Zutatenliste");
            PrintTable(ings, i => i.Name, i => i.Id);
        }

        private static T? SelectItem<T>(IEnumerable<T> items, Func<T, string> display)
        {
            var list = items.ToList();
            for (int i = 0; i < list.Count; i++)
                Console.WriteLine($"{i + 1}) {display(list[i])}");
            Console.Write("Auswahl (0 zum Abbrechen): ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int index) && index > 0 && index <= list.Count)
                return list[index - 1];
            return default;
        }

        private static string ReadPassword()
        {
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
            catch
            {
                return Console.ReadLine() ?? string.Empty;
            }
        }

        //  Print 
        private static void PrintTable<T>(IEnumerable<T> items, params Func<T, object>[] columns)
        {
            var list = items.ToList();
            if (!list.Any())
            {
                Console.WriteLine("(keine Daten)");
                return;
            }

            var table = list.Select(item => columns.Select(c => c(item)?.ToString() ?? "").ToList()).ToList();
            var colWidths = Enumerable.Range(0, columns.Length)
                                      .Select(i => table.Max(row => row[i].Length))
                                      .ToList();

            Console.WriteLine("┌" + string.Join("┬", colWidths.Select(w => new string('─', w + 2))) + "┐");
            foreach (var row in table)
            {
                for (int i = 0; i < row.Count; i++)
                    Console.Write("│ " + row[i].PadRight(colWidths[i]) + " ");
                Console.WriteLine("│");
            }
            Console.WriteLine("└" + string.Join("┴", colWidths.Select(w => new string('─', w + 2))) + "┘");
        }

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

        // View 
        private static void ShowSummary()
        {
            Console.Clear();
            PrintHeader("Zusammenfassung vor Programmende");

            Console.WriteLine("\n Benutzerübersicht:");
            if (_context.Users.Count == 0)
                Console.WriteLine("(Keine Benutzer registriert)");
            else
                PrintTable(_context.Users, u => u.UserName, u => u.Id);

            Console.WriteLine("\n Kategorien:");
            if (_context.Categories.Count == 0)
                Console.WriteLine("(Keine Kategorien)");
            else
                PrintTable(_context.Categories, c => c.Name, c => c.Id);

            Console.WriteLine("\n Zutaten:");
            if (_context.Ingredients.Count == 0)
                Console.WriteLine("(Keine Zutaten)");
            else
                PrintTable(_context.Ingredients, i => i.Name, i => i.Id);

            Console.WriteLine("\n Rezepte:");
            if (_context.Recipes.Count == 0)
                Console.WriteLine("(Keine Rezepte)");
            else
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
            if (favs.Count == 0)
                Console.WriteLine("(Keine Favoriten)");
            else
                PrintTable(favs, f => f.User, f => f.Recipe);

            Console.WriteLine("\n═══════════════════════════════════════════");
            Console.WriteLine("Programmende – Daten wurden gespeichert ");
            Console.WriteLine("═══════════════════════════════════════════");
            Console.WriteLine("Zum Schließen eine beliebige Taste drücken...");
            Console.ReadKey();
        }
    }
}
