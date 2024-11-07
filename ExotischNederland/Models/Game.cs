using ExotischNederland.DAL;
using System.Collections.Generic;
using ExotischNederland.Models;
using System.Runtime.CompilerServices;
using System.Linq;

namespace ExotischNederland.Models
{
    internal class Game
    {
        public int Id { get; private set; }
        public Route Route { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public List<Question> Questions
        {
            get
            {
                return LoadQuestions();
            }
            private set
            {
                Questions = value;
            }
        }

        public Game(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Route = Route.Find((int)_values["RouteId"]);
            this.Title = (string)_values["Title"];
            this.Description = (string)_values["Description"];
        }

        public static Game Create(Route route, string title, string description)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "RouteId", route.Id },
                { "Title", title },
                { "Description", description }
            };
            int id = db.Insert("Game", values);
            values["Id"] = id; // Add the generated Id to the values dictionary
            return new Game(values);
        }

        public static Game Find(int gameId)
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Find<Game>("Id", gameId.ToString());
        }

        public void Update(string newTitle, string newDescription)
        {
            Title = newTitle;
            Description = newDescription;

            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "Title", newTitle },
                { "Description", newDescription }
            };
            db.Update("Game", this.Id, values);
        }

        public void Delete(User _authenticatedUser)
        {
            if (!_authenticatedUser.Permission.CanManageGames()) return;
            foreach (var question in Questions) question.Delete(_authenticatedUser);

            SQLDAL db = SQLDAL.Instance;
            db.Delete("Game", this.Id);
        }

        public static List<Game> GetAll()
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Select<Game>(); // Fetch all games or add criteria for playable games
        }

        private List<Question> LoadQuestions()
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Select<Question>(qb => qb.Where("GameId", "=", this.Id));
        }

        public int UserScore(User _user)
        {
            int score = 0;
            List<UserQuest> userQuests = _user.UserQuests();
            foreach (Question question in Questions)
            {
                if (userQuests.Any(uq => uq.Question.Id == question.Id && uq.Answer.Correct))
                {
                    score += 1;
                }
            }
            return score;
        }

        public void ResetUserScore(User _user)
        {
            List<UserQuest> userQuests = _user.UserQuests();
            foreach (Question question in Questions)
            {
                userQuests.First(uq => uq.Question.Id == question.Id).Delete();
            }
        }
    }
}