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
                menuItems.Add("viewAllGames", "View all games");
                menuItems.Add("createGame", "Create a new game");
            }
            if (authenticatedUser.Permission.CanPlayGames())
            {
                menuItems.Add("playGame", "Play a game");
            }
            menuItems.Add("back", "Return to main menu");
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

            Console.WriteLine("Available Games:");
            if (games.Count > 0)
            {
                var gameOptions = new Dictionary<string, string>();
                foreach (var game in games)
                {
                    gameOptions[game.Id.ToString()] = $"{game.Title} - {game.Description}";
                }
                gameOptions.Add("back", "Return to main menu");

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
                Console.WriteLine("No games available.");
                Console.ReadKey();
            }
        }

        private void ViewGameDetails(Game game)
        {
            Console.Clear();
            Console.WriteLine("Game Details:");
            Console.WriteLine($"Title: {game.Title}");
            Console.WriteLine($"Description: {game.Description}");
            Console.WriteLine($"Route ID: {game.Route.Id}");

            var options = new Dictionary<string, string>
            {
                { "back", "Return to game list" }
            };

            if (authenticatedUser.Permission.CanManageGames())
            {
                options.Add("edit", "Edit this game");
                options.Add("delete", "Delete this game");
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
                new FormField("title", "New title (leave empty to keep)", "string", false, game.Title),
                new FormField("description", "New description (leave empty to keep)", "string", false, game.Description)
            };

            Dictionary<string, object> values = new Form(fields).Prompt();
            if (values == null) return;

            game.Update((string)values["title"], (string)values["description"]);
            Console.WriteLine("Game updated successfully.");
            Console.ReadKey();
        }

        private void DeleteGame(Game game)
        {
            Console.Clear();
            Console.WriteLine($"Are you sure you want to delete the game '{game.Title}'? [Y/N]");
            ConsoleKey confirmation = Console.ReadKey().Key;

            if (confirmation == ConsoleKey.Y)
            {
                Game.Delete(game.Id);
                Console.WriteLine("\nGame deleted successfully.");
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
                new FormField("title", "Enter game title", "string", true),
                new FormField("description", "Enter game description", "string", true),
                new FormField("routeId", "Enter route ID for the game", "number", true)
            };

            var values = new Form(fields).Prompt();
            if (values == null) return;

            int routeId = (int)values["routeId"];
            Route route = Route.Find(routeId);
            if (route != null)
            {
                Game game = Game.Create(route, (string)values["title"], (string)values["description"]);
                Console.WriteLine($"Game '{values["title"]}' created successfully.");
            }
            else
            {
                Console.WriteLine("Invalid route ID.");
            }
            Console.ReadKey();
        }

        private void PlayGame()
        {
            Console.Clear();
            Console.WriteLine("Available Games to Play:");
            List<Game> games = Game.GetAllPlayableGames();
            int i = 1;
            foreach (var game in games)
            {
                Console.WriteLine($"{i}. {game.Title} - {game.Description}");
                i++;
            }

            Console.WriteLine("Select a game number to play or enter '0' to return:");
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