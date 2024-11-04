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

        private UserQuest(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Question = Question.Find((int)_values["QuestionId"]);
            this.User = User.Find((int)_values["UserId"]);
            this.Answer = Answer.Find((int)_values["AnswerId"]);
        }

        public static UserQuest Create(Question question, Answer answer, User user)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "QuestionId", question.Id },
                { "AnswerId", answer.Id },
                { "UserId", user.Id }
            };
            int id = db.Insert("UserQuest", values);
            return Find(id);
        }

        public static UserQuest Find(int userQuestId)
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Find<UserQuest>("UserQuest", userQuestId.ToString());
        }

        public void Update(Question newQuestion, Answer newAnswer)
        {
            Question = newQuestion;
            Answer = newAnswer;

            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "QuestionId", newQuestion.Id },
                { "AnswerId", newAnswer.Id }
            };
            db.Update("UserQuest", this.Id, values);
        }

        public static void Delete(int userQuestId)
        {
            SQLDAL db = SQLDAL.Instance;
            db.Delete("UserQuest", userQuestId);
        }
    }
}