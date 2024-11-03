using ExotischNederland.DAL;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
    internal class Answer
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public int QuestionId => Question?.Id ?? 0; //  Question niet null is
        public string Text { get; set; }
        public bool Correct { get; set; }

       
        private Answer() { }

        public Answer(Dictionary<string, object> values)
        {
            Id = (int)values["Id"];
            Text = (string)values["Text"];
            Correct = (bool)values["Correct"];
            Question = Question.Find((int)values["QuestionId"]); // Vindt de bijbehorende vraag
        }

        // Static factory method to create a new Answer and save it to the database
        public static Answer CreateAnswer(Question _question, string _text, bool _correct)
        {
            SQLDAL db = new SQLDAL();
            var values = new Dictionary<string, object>
            {
                { "QuestionId", _question.Id },
                { "Text", _text },
                { "Correct", _correct }
            };
            int id = db.Insert("Answer", values);

            return Find(id);
        }

        //  op te halen uit de database
        public static Answer Find(int _answerId)
        {
            SQLDAL db = new SQLDAL();
            var values = db.Find<Dictionary<string, object>>("Answer", _answerId.ToString());

            if (values != null)
            {
                return new Answer(values);
            }

            return null;
        }
    }
}
