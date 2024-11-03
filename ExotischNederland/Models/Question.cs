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
        public List<Answer> Answers { get; private set; }

        private Question(int id, Game game, string text, string type)
        {
            Id = id;
            Game = game;
            Text = text;
            Type = type;
            Answers = LoadAnswers(); // Load answers from the database
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
            return Find(id);
        }

        public static Question Find(int questionId)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = db.Find<Dictionary<string, object>>("Question", questionId.ToString());

            return values != null
                ? new Question(
                    (int)values["Id"],
                    Game.Find((int)values["GameId"]),
                    (string)values["Text"],
                    (string)values["Type"]
                )
                : null;
        }

        public void AddAnswer(Answer answer)
        {
            if (!Answers.Exists(a => a.Id == answer.Id))
            {
                Answers.Add(answer);
                SQLDAL db = SQLDAL.Instance;
                var values = new Dictionary<string, object>
                {
                    { "QuestionId", this.Id },
                    { "AnswerId", answer.Id }
                };
                db.Insert("QuestionAnswer", values);
            }
        }

        public void RemoveAnswer(int answerId)
        {
            Answers.RemoveAll(a => a.Id == answerId);
            SQLDAL db = SQLDAL.Instance;
            db.Delete("QuestionAnswer", qb => qb
                .Where("QuestionId", "=", this.Id)
                .Where("AnswerId", "=", answerId));
        }

        private List<Answer> LoadAnswers()
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Select<Answer>(qb => qb.Where("QuestionId", "=", this.Id));
        }
    }
}