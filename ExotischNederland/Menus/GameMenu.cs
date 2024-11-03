using System;
using System.Collections.Generic;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class GameMenu
    {
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public GameMenu(User _authenticatedUser)
        {
            this.authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            Dictionary<string, string> menuItems = new Dictionary<string, string>();

            // Add game management options if the user has the "Beheerder" role
            if (authenticatedUser.Permission.CanManageGames())
            {
                menuItems.Add("createGame", "Create a new game");
                menuItems.Add("editGame", "Edit an existing game");
                menuItems.Add("deleteGame", "Delete a game");
            }

            // Add game-playing option for "Familie" and "Kinderen" roles
            if (authenticatedUser.Permission.CanPlayGames())
            {
                menuItems.Add("playGame", "Play a game");
            }

            menuItems.Add("back", "Return to main menu");
            return menuItems;
        }

        public void Show()
        {
            menuItems = this.GetMenuItems();
            while (true)
            {
                string selected = Helpers.MenuSelect(this.menuItems, true);

                if (selected == "createGame")
                {
                    CreateGame();
                }
                else if (selected == "editGame")
                {
                    EditGame();
                }
                else if (selected == "deleteGame")
                {
                    DeleteGame();
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

        private void CreateGame()
        {
            Console.Clear();
            Console.WriteLine("Create a New Game");
            Console.Write("Enter the title of the game: ");
            string title = Console.ReadLine();
            Console.Write("Enter the description of the game: ");
            string description = Console.ReadLine();

            Console.Write("Enter route ID for the game: ");
            int routeId;
            if (int.TryParse(Console.ReadLine(), out routeId))
            {
                Route route = Route.Find(routeId);
                if (route != null)
                {
                    Game game = Game.Create(route, title, description);
                    Console.WriteLine($"Game '{title}' created successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid route ID. Game creation failed.");
                }
            }
            else
            {
                Console.WriteLine("Invalid route ID input.");
            }

            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        private void EditGame()
        {
            Console.Clear();
            Console.WriteLine("Edit a Game");
            Console.Write("Enter the ID of the game to edit: ");
            int gameId;
            if (int.TryParse(Console.ReadLine(), out gameId))
            {
                Game game = Game.Find(gameId);

                if (game != null)
                {
                    Console.Write("New title (leave empty to keep current): ");
                    string newTitle = Console.ReadLine();
                    Console.Write("New description (leave empty to keep current): ");
                    string newDescription = Console.ReadLine();

                    game.Update(newTitle, newDescription);
                    Console.WriteLine("Game updated successfully.");
                }
                else
                {
                    Console.WriteLine("Game not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid game ID input.");
            }

            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        private void DeleteGame()
        {
            Console.Clear();
            Console.WriteLine("Delete a Game");
            Console.Write("Enter the ID of the game to delete: ");
            int gameId;
            if (int.TryParse(Console.ReadLine(), out gameId))
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
            }
            else
            {
                Console.WriteLine("Invalid game ID input.");
            }

            Console.WriteLine("Press any key to return to the menu.");
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
                if (int.TryParse(Console.ReadLine(), out int answerChoice) && answerChoice > 0 && answerChoice <= question.Answers.Count)
                {
                    // Logic to handle answer choice (e.g., check correctness)
                }
            }

            Console.WriteLine("\nEnd of game. Press any key to return to the menu.");
            Console.ReadKey();
        }
    }
}