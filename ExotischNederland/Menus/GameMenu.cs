using System;
using System.Collections.Generic;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class GameMenu : IMenu
    {
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public GameMenu(User _authenticatedUser)
        {
            this.authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            var menuItems = new Dictionary<string, string>();

            if (authenticatedUser.Permission.CanManageGames())
            {
                menuItems.Add("viewAllGames", "Bekijk alle spellen");
                menuItems.Add("createGame", "Maak een nieuw spel");
            }
            if (authenticatedUser.Permission.CanPlayGames())
            {
                menuItems.Add("playGame", "Speel een spel");
            }
            menuItems.Add("back", "Keer terug naar het hoofdmenu");
            return menuItems;
        }

        public void Show()
        {
            menuItems = GetMenuItems();
            while (true)
            {
                string selected = Helpers.MenuSelect(menuItems, true);

                if (selected == "viewAllGames")
                {
                    ViewAllGames();
                }
                else if (selected == "createGame")
                {
                    CreateGame();
                }
                else if (selected == "playGame")
                {
                    PlayGame();
                }
                else if (selected == "back")
                {
                    break;
                }
            }
        }

        private void ViewAllGames()
        {
            Console.Clear();
            List<Game> games = Game.GetAllPlayableGames();

            Console.WriteLine("Beschikbare spellen:");
            if (games.Count > 0)
            {
                var gameOptions = new Dictionary<string, string>();
                foreach (var game in games)
                {
                    gameOptions[game.Id.ToString()] = $"{game.Title} - {game.Description}";
                }
                gameOptions.Add("back", "Keer terug naar het hoofdmenu");

                string selectedGameId = Helpers.MenuSelect(gameOptions, false);
                if (selectedGameId != "back")
                {
                    int gameId = int.Parse(selectedGameId);
                    Game game = games.Find(g => g.Id == gameId);
                    ViewGameDetails(game);
                }
            }
            else
            {
                Console.WriteLine("Geen spellen beschikbaar");
                Console.ReadKey();
            }
        }

        private void ViewGameDetails(Game game)
        {
            Console.Clear();
            Console.WriteLine("Spel Details:");
            Console.WriteLine($"Titel: {game.Title}");
            Console.WriteLine($"Beschrijving: {game.Description}");
            Console.WriteLine($"Route ID: {game.Route.Id}");

            var options = new Dictionary<string, string>
            {
                { "back", "Terug naar de spellenlijst" }
            };

            if (authenticatedUser.Permission.CanManageGames())
            {
                options.Add("edit", "Bewerk dit spel");
                options.Add("delete", "Verwijder dit spel");
            }

            string selectedOption = Helpers.MenuSelect(options, true);

            if (selectedOption == "edit")
            {
                EditGame(game);
            }
            else if (selectedOption == "delete")
            {
                DeleteGame(game);
            }
        }

        private void EditGame(Game game)
        {
            Console.Clear();
            List<FormField> fields = new List<FormField>
            {
                new FormField("title", "Nieuwe titel (leeg laten om te behouden)", "string", false, game.Title),
                new FormField("description", "Nieuwe beschrijving (leeg laten om te behouden)", "string", false, game.Description)
            };

            Dictionary<string, object> values = new Form(fields).Prompt();
            if (values == null) return;

            game.Update((string)values["title"], (string)values["description"]);
            Console.WriteLine("Spel is succesvol bijgewerkt.");
            Console.ReadKey();
        }

        private void DeleteGame(Game game)
        {
            Console.Clear();
            Console.WriteLine($"Weet je zeker dat je het spel wilt verwijderen '{game.Title}'? [Y/N]");
            ConsoleKey confirmation = Console.ReadKey().Key;

            if (confirmation == ConsoleKey.Y)
            {
                Game.Delete(game.Id);
                Console.WriteLine("Spel succesvol verwijderd.");
                Console.ReadKey();
            }
            else
            {
                ViewGameDetails(game);
            }
        }

        private void CreateGame()
        {
            Console.Clear();
            var fields = new List<FormField>
            {
                new FormField("title", "Voer de speltitel in", "string", true),
                new FormField("description", "Voer de spelbeschrijving in", "string", true),
                new FormField("routeId", "Voer de route-ID voor het spel in", "number", true)
            };

            var values = new Form(fields).Prompt();
            if (values == null) return;

            int routeId = (int)values["routeId"];
            Route route = Route.Find(routeId);
            if (route != null)
            {
                Game game = Game.Create(route, (string)values["title"], (string)values["description"]);
                Console.WriteLine($"Spel '{values["title"]}' succesvol aangemaakt.");
            }
            else
            {
                Console.WriteLine("Ongeldige route ID.");
            }
            Console.ReadKey();
        }

        private void PlayGame()
        {
            Console.Clear();
            Console.WriteLine("Beschikbare Spellen om te Spelen:");
            List<Game> games = Game.GetAllPlayableGames();
            int i = 1;
            foreach (var game in games)
            {
                Console.WriteLine($"{i}. {game.Title} - {game.Description}");
                i++;
            }

            Console.WriteLine("Selecteer een spelnummer om te spelen of voer '0' in om terug te keren:");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= games.Count)
            {
                StartGame(games[choice - 1]);
            }
        }

        private void StartGame(Game game)
        {
            Console.Clear();
            Console.WriteLine($"Playing Game: {game.Title}");
            Console.WriteLine(game.Description);

            foreach (var question in game.Questions)
            {
                Console.WriteLine($"\nQuestion: {question.Text}");
                int j = 1;
                foreach (var answer in question.Answers)
                {
                    Console.WriteLine($"{j}. {answer.Text}");
                    j++;
                }

                Console.WriteLine("Select an answer:");
                int.TryParse(Console.ReadLine(), out int answerChoice);
            }
            Console.WriteLine("\nEnd of game. Press any key to return.");
            Console.ReadKey();
        }
    }
}