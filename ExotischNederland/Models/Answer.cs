using ExotischNederland.DAL;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
    internal class Answer
    {
        public int Id { get; private set; }
        public Question Question { get; private set; }
        public string Text { get; private set; }
        public bool Correct { get; private set; }

        private Answer(Dictionary<string, object> _values)
        {
            Id = (int)_values["Id"];
            Question = Question.Find((int)_values["QuestionId"]);
            Text = (string)_values["Text"];
            Correct = (bool)_values["Correct"];
        }

        public static Answer Create(Question question, string text, bool correct)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "QuestionId", question.Id },
                { "Text", text },
                { "Correct", correct }
            };
            int id = db.Insert("Answer", values);
            return Find(id);
        }

        public static Answer Find(int answerId)
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Find<Answer>("Answer", answerId.ToString());
        }

        public void Update(string newText, bool isCorrect)
        {
            Text = newText;
            Correct = isCorrect;

            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "Text", newText },
                { "Correct", isCorrect }
            };
            db.Update("Answer", this.Id, values);
        }

        public static void Delete(int answerId)
        {
            SQLDAL db = SQLDAL.Instance;
            db.Delete("Answer", answerId);
        }
    }
}