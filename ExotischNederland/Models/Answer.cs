using ExotischNederland.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    internal class Answer
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public string Text { get; set; }
        public bool Correct { get; set; }

        // Static factory method to create a new Answer and save it to the database
        public static Answer CreateAnswer(Question _question, string _text, bool _correct)
        {
            // Save to database
            SQLDAL db = new SQLDAL();
            var values = new Dictionary<string, object>
            {
                { "QuestionId", _question.Id },
                { "Text", _text },
                { "Correct", _correct }
            };
            int id = db.Insert("Answer", values);

            return db.Find<Answer>("Id", id.ToString());
        }
    }
}
