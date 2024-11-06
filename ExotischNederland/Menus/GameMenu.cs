using System;
using System.Collections.Generic;
using ExotischNederland.Models;
using System.Linq;
using System.Security.Cryptography;

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

            menuItems.Add("viewAllGames", "Bekijk alle spellen");
            if (authenticatedUser.Permission.CanManageGames())
            {
                menuItems.Add("createGame", "Create a new game");
            }
            menuItems.Add("back", "Return to main menu");
            return menuItems;
        }

        public void Show()
        {
            menuItems = GetMenuItems();
            string selected = Helpers.MenuSelect(menuItems, true);

            if (selected == null) return;
            else if (selected == "viewAllGames")
            {
                ViewAllGames();
            }
            else if (selected == "createGame")
            {
                CreateGame();
            }
            Show();
        }

        private void ViewAllGames()
        {
            Console.Clear();
            List<Game> games = Game.GetAll();

            Console.WriteLine("Beschikbare spellen:");
            if (games.Count > 0)
            {
                Dictionary<string, string> options = games.ToDictionary(x => x.Id.ToString(), x => $"{x.Id}. {x.Title}: {x.Description}");
                options.Add("back", "Ga terug");

                string selectedGameId = Helpers.MenuSelect(options, false, new List<string> { "Alle spellen:"});
                if (selectedGameId == "back" || selectedGameId == null) return;
                else
                {
                    Game game = games.Find(g => g.Id == int.Parse(selectedGameId));
                    ViewGameDetails(game);
                }
            }
            else
            {
                Console.WriteLine("Geen spellen beschikbaar");
                Console.ReadKey();
            }
        }

        private void ViewGameDetails(Game _game)
        {
            Console.Clear();
            List<string> text = new List<string>
            {
                "Game Details:",
                $"Title: {_game.Title}",
                $"Description: {_game.Description}",
                $"Route: {_game.Route.Name}"
            };
            int userScore = _game.UserScore(this.authenticatedUser);
            float percentage = (float)userScore / _game.Questions.Count * 100;
            if (userScore > 0)
            {
                text.Add($"Resultaat: {percentage}%");
            }


            Dictionary<string, string> options = new Dictionary<string, string>();

            if (authenticatedUser.Permission.CanPlayGames())
            {
                options.Add("play", "Speel dit spel");
            }

            if (authenticatedUser.Permission.CanManageGames())
            {
                options.Add("edit", "Spel bewerken");
                options.Add("questions", "Vragen beheren");
                options.Add("delete", "Spel verwijderen");
            }

            string selectedOption = Helpers.MenuSelect(options, true, text);
            
            if (selectedOption == "play")
            {
                StartGame(_game);
                ViewGameDetails(_game);
            } 
            else if (selectedOption == "edit")
            {
                EditGame(_game);
                ViewGameDetails(_game);
            }
            else if (selectedOption == "questions")
            {
                QuestionMenu qm = new QuestionMenu(_game, this.authenticatedUser);
                qm.Show();
                ViewGameDetails(_game);
            }
            else if (selectedOption == "delete")
            {
                DeleteGame(_game);
            }
            ViewAllGames();
        }

        private void EditGame(Game _game)
        {
            Console.Clear();
            List<FormField> fields = new List<FormField>
            {
                new FormField("title", "Nieuwe naam", "string", false, _game.Title),
                new FormField("description", "Nieuwe beschrijving", "string", false, _game.Description)
            };

            Dictionary<string, object> values = new Form(fields).Prompt();
            if (values == null) return;

            _game.Update((string)values["title"], (string)values["description"]);
            Console.WriteLine("Spel bijgewerkt");
            Console.ReadKey();
        }

        private void DeleteGame(Game _game)
        {
            Console.Clear();
            Console.WriteLine($"Weet je zeker dat je het spel '{_game.Title}' wilt verwijderen? [J/N]");
            
            if (Helpers.ConfirmPrompt())
            {
                _game.Delete(authenticatedUser);
                Console.WriteLine("Spel verwijderd");
                Console.ReadKey();
            }
            else
            {
                ViewGameDetails(_game);
            }
        }

        private void CreateGame()
        {
            Console.Clear();
            Dictionary<string, string> routeOptions = new Dictionary<string, string>();
            List<Route> routes = Route.GetAll();
            if (routes.Count > 0)
            {
                routeOptions = routes.ToDictionary(x => x.Id.ToString(), x => $"{x.Id}. {x.Name}");
            }
            else
            {
                Console.WriteLine("Er zijn geen routes beschikbaar. Maak eerst een route aan.");
                Console.ReadKey();
                return;
            }
            List<FormField> fields = new List<FormField>
            {
                new FormField("title", "Naam", "string", true),
                new FormField("description", "Beschrijving", "string", true),
                new FormField("routeId", "Selecteer een route", "single_select", true, null, routeOptions)
            };

            Dictionary<string, object> values = new Form(fields).Prompt();
            if (values == null) return;

            int routeId = int.Parse(values["routeId"].ToString());
            Route route = Route.Find(routeId);
            
            Game game = Game.Create(route, (string)values["title"], (string)values["description"]);
            Console.WriteLine($"Spel '{values["title"]}' aangemaakt");
            new QuestionMenu(game, this.authenticatedUser).Show();
            ViewGameDetails(game);
        }

        private void StartGame(Game _game)
        {
            Console.Clear();
            int userScore = _game.UserScore(this.authenticatedUser);
            if (userScore > 0)
            {
                Console.WriteLine("Je hebt dit spel eerder gespeeld. Opnieuw spelen zal de score wissen. Doorgaan? [J/N]");
                if (!Helpers.ConfirmPrompt()) return;
                _game.ResetUserScore(this.authenticatedUser);
            }
            List<string> text = new List<string>
            {
                _game.Title,
                _game.Description
            };
            int numberCorrect = 0;
            foreach (Question question in _game.Questions)
                if (this.AskQuestion(question)) numberCorrect++;
            
            Console.Clear();
            Console.WriteLine("Einde van het spel!");
            Console.WriteLine($"Je hebt {numberCorrect} van de {_game.Questions.Count} vragen goed beantwoord.");
            Console.ReadKey();
        }

        private bool AskQuestion(Question _question)
        {
            Console.Clear();
            FormField field;
            if (_question.Type == "multipleChoice")
            {
                Dictionary<string, string> options = new Dictionary<string, string>();
                foreach (Answer answer in _question.Answers)
                {
                    options.Add(answer.Id.ToString(), answer.Text);
                }
                field = new FormField("answer", _question.Text, "single_select", true, null, options);
            }
            else
            {
                field = new FormField("answer", _question.Text, _question.Type, true);
            }
            object result = field.Input();
            Answer givenAnswer = CheckAnswer(_question, result);

            return UserQuest.Create(_question, givenAnswer, this.authenticatedUser).Correct;
        }

        private Answer CheckAnswer(Question _question, object _result)
        {
            switch (_question.Type)
            {
                case "boolean":
                    // Return the answer
                    return _question.Answers.Find(a => a.Text == ((bool)_result ? "T" : "F"));
                case "multipleChoice":
                    // Return the answer if it exists and is correct, otherwise return null.
                    return _question.Answers.Find(a => a.Id == int.Parse((string)_result));
                default:
                    // This should never happen. If it does, return false.
                    return _question.Answers.Find(a => a.Text.ToLower() == _result.ToString().ToLower()) ?? Answer.Create(_question, (string)_result, false);
            }
        }
    }
}