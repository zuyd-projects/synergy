using ExotischNederland.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    internal class Game
    {
        public int Id { get; set; }
        public Route Route { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }

        // Static factory method to create a new Game and save it to the database
        public static Game Create(Route _route, string _title, string _description)
        {
            // Save to database
            SQLDAL db = new SQLDAL();
            var values = new Dictionary<string, object>
            {
                { "RouteId", _route.Id },
                { "Title", _title },
                { "Description", _description }
            };
            int id = db.Insert("Game", values);

            return Find(id);
        }

        //public static Game Find(int _gameId)
        //{
        //    SQLDAL db = new SQLDAL();
        //    return db.Find<Game>("Id", _gameId.ToString());
        //}
        // Private method to find and initialize a Game by Id
        public static Game Find(int gameId)
        {
            SQLDAL db = new SQLDAL();
            var values = db.Find<Dictionary<string, object>>("Game", gameId.ToString());

            if (values != null)
            {
                // Gebruik de privé constructor om een nieuw Game object te initialiseren
                Game game = new Game
                {
                    Id = (int)values["Id"],
                    Route = new Route { Id = (int)values["RouteId"] }, // Route initialisatie
                    Title = (string)values["Title"],
                    Description = (string)values["Description"],
                    Questions = new List<Question>() // Kan later worden geladen als dat nodig is
                };

                return game;
            }

            return null; // Return null als de game niet gevonden wordt
        }

        // Maak de default constructor private om directe instanties te voorkomen
        private Game() { }

        public void AddQuestion(Question question)
        {
            Questions.Add(question);
        }

        public void RemoveQuestion(int questionId)
        {
            Questions.RemoveAll(q => q.Id == questionId);
        }
    }
}
