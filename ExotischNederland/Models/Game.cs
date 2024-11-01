using ExotischNederland.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    public class Game
    {
        public int Id { get; set; }
        public Route Route { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }

        // Static factory method to create a new Game and save it to the database
        public static Game CreateGame(int routeId, string title, string description)
        {
            Route route = new Route { Id = routeId };
            Game newGame = new Game
            {
                Route = route,
                Title = title,
                Description = description,
                Questions = new List<Question>()
            };

            // Save to database
            SQLDAL db = new SQLDAL();
            var values = new Dictionary<string, object>
        {
            { "RouteId", routeId },
            { "Title", title },
            { "Description", description }
        };
            newGame.Id = db.Insert("Game", values);

            return newGame;
        }

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
