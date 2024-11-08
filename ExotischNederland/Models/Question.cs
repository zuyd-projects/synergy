using ExotischNederland.DAL;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
    internal class Question
    {
        public int Id { get; private set; }
        public Game Game { get; private set; }
        public string Text { get; private set; }
        public string Type { get; private set; } // "multiple choice" or "open"
        public List<Answer> Answers
        {
            get
            {
                return LoadAnswers();
            }
            private set
            {
                Answers = value;
            }
        }

        public Question(Dictionary<string, object> _values)
        {
            Id = (int)_values["Id"];
            Game = Game.Find((int)_values["GameId"]);
            Text = (string)_values["Text"];
            Type = (string)_values["Type"];
        }

        public static Question Create(Game game, string text, string type)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "GameId", game.Id },
                { "Text", text },
                { "Type", type }
            };
            int id = db.Insert("Question", values);
            values["Id"] = id; // Add the generated Id to the values dictionary
            return new Question(values);
        }

        public static Question Find(int questionId)
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Find<Question>("Id", questionId.ToString());
        }

        private List<Answer> LoadAnswers()
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Select<Answer>(qb => qb.Where("QuestionId", "=", this.Id));
        }

        public void Delete(User _authenticatedUser)
        {
            if (!_authenticatedUser.Permission.CanManageGames()) return;

            foreach (var answer in Answers) answer.Delete(_authenticatedUser);

            SQLDAL db = SQLDAL.Instance;
            db.Delete("Question", this.Id);
        }
    }
}