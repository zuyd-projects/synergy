using ExotischNederland.DAL;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
    internal class UserQuest
    {
        public int Id { get; private set; }
        public Question Question { get; private set; }
        public User User { get; private set; }
        public Answer Answer { get; private set; }
        public bool Correct { get { return Answer.Correct; } }

        public UserQuest(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Question = Question.Find((int)_values["QuestionId"]);
            this.User = User.Find((int)_values["UserId"]);
            this.Answer = Answer.Find((int)_values["AnswerId"]);
        }

        public static UserQuest Create(Question _question, Answer _answer, User _user)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "QuestionId", _question.Id },
                { "AnswerId", _answer.Id },
                { "UserId", _user.Id }
            };
            int id = db.Insert("UserQuest", values);
            return Find(id);
        }

        public static UserQuest Find(int userQuestId)
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Find<UserQuest>("Id", userQuestId.ToString());
        }

        public void Delete()
        {
            SQLDAL db = SQLDAL.Instance;
            db.Delete("UserQuest", this.Id);
        }
    }
}