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

        private UserQuest(int id, Question question, User user, Answer answer)
        {
            Id = id;
            Question = question;
            User = user;
            Answer = answer;
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
            var values = db.Find<Dictionary<string, object>>("UserQuest", userQuestId.ToString());

            return values != null
                ? new UserQuest(
                    (int)values["Id"],
                    Question.Find((int)values["QuestionId"]),
                    User.Find((int)values["UserId"]),
                    Answer.Find((int)values["AnswerId"])
                )
                : null;
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