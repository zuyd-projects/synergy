using ExotischNederland.DAL;
using System;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
    internal class Question
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public string Text { get; set; }
        public string Type { get; set; } // "multiple choice" or "open"
        public List<Answer> Answers { get; set; }

        private Question(Dictionary<string, object> _values)
        {
            Id = (int)_values["Id"];
            Game = Game.Find((int)_values["GameId"]);
            Text = (string)_values["Text"];
            Type = (string)_values["Type"];
            Answers = GetAnswersFromDatabase(); 
        }

        // Static factory method to create a new Question and save it to the database
        public static Question Create(Game _game, string _text, string _type)
        {
            
            SQLDAL db = new SQLDAL();
            var values = new Dictionary<string, object>
            {
                { "GameId", _game.Id },
                { "Text", _text },
                { "Type", _type }
            };
            int id = db.Insert("Question", values);

            return Find(id);
        }

        public static Question Find(int _questionId)
        {
            SQLDAL db = new SQLDAL();
            var values = db.Find<Dictionary<string, object>>("Question", _questionId.ToString());

            if (values != null)
            {
                return new Question(values);
            }

            return null;
        }

        
        private List<Answer> GetAnswersFromDatabase()
        {
            SQLDAL db = new SQLDAL();
            var answerValuesList = db.Select<Dictionary<string, object>>(qb => qb.Where("QuestionId", "=", Id.ToString()));

            List<Answer> answerList = new List<Answer>();
            foreach (var answerValues in answerValuesList)
            {
                answerList.Add(new Answer(answerValues));
            }

            return answerList;
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
