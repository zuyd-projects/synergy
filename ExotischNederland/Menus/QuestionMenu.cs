using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class QuestionMenu: IMenu
    {
        private readonly User authenticatedUser;
        private readonly Game game;
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();
        public QuestionMenu(Game _game, User _authenticatedUser) 
        {
            this.game = _game;
            this.authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            Dictionary<string, string> menuItems = new Dictionary<string, string>();
            foreach (var question in this.game.Questions)
            {
                // Assuming you want to add question text as key and question type as value
                menuItems.Add(question.Id.ToString(), $"{question.Id}. {question.Text}");
            }
            menuItems.Add("add", "Voeg vraag toe");
            return menuItems;
        }

        public void Show()
        {
            this.menuItems = this.GetMenuItems();
            string selected = Helpers.MenuSelect(this.menuItems, false, new List<string> { "Druk op ESC om terug te keren"});

            if (selected is null) return;
            if (selected == "add")
            {
                AddQuestion();
            }
            else
            {
                Question selectedQuestion = this.game.Questions.Find(x => x.Id.ToString() == selected);
                if (selectedQuestion != null)
                    ViewQuestion(selectedQuestion);
            }
            Show();
        }

        public void AddQuestion()
        {
            Console.Clear();
            Dictionary<string, string> questionTypes = new Dictionary<string, string>
            {
                { "text", "Tekst" },
                { "multipleChoice", "Meerkeuze" },
                { "boolean", "Ja/Nee" }
            };
            List<FormField> fields = new List<FormField>
            {
                new FormField("text", "Vraagtekst", "string", true),
                new FormField("type", "Vraagtype", "single_select", true, null, questionTypes),
            };
            Dictionary<string, object> results = new Form(fields).Prompt();

            if (results == null)
                return;

            Question question = Question.Create(game, results["text"].ToString(), results["type"].ToString());

            switch (results["type"])
            {
                case "text":
                    FormField answerField = new FormField("answer", "Antwoord", "string", true);
                    string answer = (string)answerField.Input();
                    Answer.Create(question, answer, true);
                    break;
                case "boolean":
                    FormField booleanField = new FormField("answer", "Antwoord", "boolean", true);
                    bool booleanAnswer = (bool)booleanField.Input();
                    Answer.Create(question, booleanAnswer ? "T" : "F", true);
                    // Also add the incorrect answer to store the user answer
                    Answer.Create(question, booleanAnswer ? "F" : "T", false);
                    break;
                case "multipleChoice":
                    int i = 1;
                    while (true)
                    {
                        FormField multipleChoiceField = new FormField("answer", $"Antwoord {i}", "string", true);
                        string multipleChoiceAnswer = (string)multipleChoiceField.Input();
                        FormField correct = new FormField("correct", "Is dit het juiste antwoord?", "boolean", true);
                        bool isCorrect = (bool)correct.Input();
                        Answer.Create(question, multipleChoiceAnswer, isCorrect);
                        Console.WriteLine("Nog een antwoord toevoegen? [J/N]");
                        if (!Helpers.ConfirmPrompt()) break;
                        i++;
                    }
                    break;
            }
        }

        public void ViewQuestion(Question _question)
        {
            List<string> text = new List<string>
            {
                _question.Text,
                $"Type: {_question.Type}"
            };
            if (_question.Answers.Count > 1)
            {
                text.Add("Antwoorden:");
                foreach (Answer answer in _question.Answers)
                {
                    string answerText = answer.Text;
                    if (answer.Correct) answerText += " (Correct)";
                    text.Add(answerText);
                }
            }
            else
            {
                text.Add("Antwoord:");
                text.Add(_question.Answers.First().Text);
            }

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "delete", "Verwijder vraag" },
                { "back", "Terug naar spel" }
            };

            string selected = Helpers.MenuSelect(options, false, text);

            if (selected == "delete")
            {
                Console.WriteLine($"Weet u zeker dat u de vraag '{_question.Text}' wilt verwijderen? [J/N]");
                if (Helpers.ConfirmPrompt())
                {
                    _question.Delete(authenticatedUser);
                    Console.WriteLine("Vraag verwijderd");
                    Console.ReadKey();
                }
                else
                {
                    ViewQuestion(_question);
                }
            }
        }
    }
}
