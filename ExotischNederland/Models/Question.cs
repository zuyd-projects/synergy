using ExotischNederland.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    public class Question
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public string Text { get; set; }
        public string Type { get; set; } // "multiple choice" or "open"
        public List<Answer> Answers { get; set; }

        private Question(int id, Game game, string text, string type)
        {
            Id = id;
            Game = game;
            Text = text;
            Type = type;
            Answers = new List<Answer>();
        }

        // Static factory method to create a new Question and save it to the database
        public static Question CreateQuestion(Game game, string text, string type)
        {
            Question newQuestion = new Question(0, game, text, type);

            // Save to database
            SQLDAL db = new SQLDAL();
            var values = new Dictionary<string, object>
        {
            { "GameId", game.Id },
            { "Text", text },
            { "Type", type }
        };
            newQuestion.Id = db.Insert("Question", values);  

            return newQuestion;
        }

        public void AddAnswer(Answer answer)
        {
            Answers.Add(answer);
        }

        public void RemoveAnswer(int answerId)
        {
            Answers.RemoveAll(a => a.Id == answerId);
        }
    }
}
