using ExotischNederland.DAL;
using System.Collections.Generic;
using ExotischNederland.Models;

namespace ExotischNederland.Models
{
    internal class Game
    {
        public int Id { get; private set; }
        public Route Route { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public List<Question> Questions { get; private set; }

        private Game(int id, Route route, string title, string description)
        {
            Id = id;
            Route = route;
            Title = title;
            Description = description;
            Questions = LoadQuestions(); // Lazy loading of questions
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
            return Find(id); // Return the created game by finding it with its new ID
        }

        public static Game Find(int gameId)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = db.Find<Dictionary<string, object>>("Game", gameId.ToString());

            return values != null
                ? new Game(
                    (int)values["Id"],
                    Route.Find((int)values["RouteId"]),
                    (string)values["Title"],
                    (string)values["Description"]
                )
                : null;
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

        public static void Delete(int gameId)
        {
            SQLDAL db = SQLDAL.Instance;
            db.Delete("Game", gameId);
        }

        public static List<Game> GetAllPlayableGames()
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Select<Game>(qb => qb); // Fetch all games or add criteria for playable games
        }

        private List<Question> LoadQuestions()
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Select<Question>(qb => qb.Where("GameId", "=", this.Id));
        }
    }
}