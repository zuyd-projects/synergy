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

        public static Game Find(int _gameId)
        {
            SQLDAL db = new SQLDAL();
            return db.Find<Game>("Id", _gameId.ToString());
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
