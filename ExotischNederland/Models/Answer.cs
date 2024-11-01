using ExotischNederland.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public string Text { get; set; }
        public bool Correct { get; set; }

        // Static factory method to create a new Answer and save it to the database
        public static Answer CreateAnswer(Question question, string text, bool correct)
        {
            Answer newAnswer = new Answer
            {
                Question = question,
                Text = text,
                Correct = correct
            };

            // Save to database
            SQLDAL db = new SQLDAL();
            var values = new Dictionary<string, object>
        {
            { "QuestionId", question.Id },
            { "Text", text },
            { "Correct", correct }
        };
            newAnswer.Id = db.Insert("Answer", values);

            return newAnswer;
        }
    }
}
