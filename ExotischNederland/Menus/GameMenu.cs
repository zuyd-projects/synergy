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
            authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            var menuItems = new Dictionary<string, string>();
            if (authenticatedUser.Permission.CanManageGames())
            {
                menuItems.Add("createGame", "Create a new game");
                menuItems.Add("editGame", "Edit an existing game");
                menuItems.Add("deleteGame", "Delete a game");
            }
            if (authenticatedUser.Permission.CanPlayGames()) menuItems.Add("playGame", "Play a game");
            menuItems.Add("back", "Return to main menu");
            return menuItems;
        }

        public void Show()
        {
            menuItems = GetMenuItems();
            while (true)
            {
                string selected = Helpers.MenuSelect(menuItems, true);

                if (selected == "createGame") CreateGame();
                else if (selected == "editGame") EditGame();
                else if (selected == "deleteGame") DeleteGame();
                else if (selected == "playGame") PlayGame();
                else if (selected == "back") break;
            }
        }

        private void CreateGame()
        {
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

        private void EditGame()
        {
            Console.Clear();
            Console.WriteLine("Edit a Game");
            Console.Write("Enter game ID: ");
            if (int.TryParse(Console.ReadLine(), out int gameId))
            {
                Game game = Game.Find(gameId);
                if (game == null)
                {
                    Console.WriteLine("Game not found.");
                    Console.ReadKey();
                    return;
                }

                var fields = new List<FormField>
                {
                    new FormField("title", "New title (leave empty to keep)", "string", false, game.Title),
                    new FormField("description", "New description (leave empty to keep)", "string", false, game.Description)
                };
                var values = new Form(fields).Prompt();
                if (values == null) return;

                game.Update((string)values["title"], (string)values["description"]);
                Console.WriteLine("Game updated successfully.");
                Console.ReadKey();
            }
        }

        private void DeleteGame()
        {
            Console.Clear();
            Console.Write("Enter game ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int gameId))
            {
                Game game = Game.Find(gameId);
                if (game != null)
                {
                    Game.Delete(gameId);
                    Console.WriteLine("Game deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Game not found.");
                }
                Console.ReadKey();
            }
        }

        private void PlayGame()
        {
            Console.Clear();
            Console.WriteLine("Available Games:");
            List<Game> games = Game.GetAllPlayableGames();
            int i = 1;
            foreach (var game in games)
            {
                Console.WriteLine($"{i}. {game.Title} - {game.Description}");
                i++;
            }
            Console.WriteLine("Enter game number to play or '0' to return:");
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
            Console.WriteLine("\nGame over. Press any key to return.");
            Console.ReadKey();
        }
    }
}