using Kaxut_new.Models;
using System;
using System.Collections.ObjectModel;

namespace Kaxut_new
{
    public sealed class Kahoot
    {
        private readonly ObservableCollection<Question> _questions = new ObservableCollection<Question>();
        public ObservableCollection<Question> Questions => _questions;

        public void AddQuestion(MultipleChoiceQuestion question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            _questions.Add(question);
        }

        public void AddQuestion(string text, string[] answers, int correctIndex)
        {
            var question = new MultipleChoiceQuestion(text, answers, correctIndex);
            _questions.Add(question);
        }

        public void AddQuestion(string text, string answer0, string answer1, string answer2, string answer3, int correctIndex)
        {
            var answers = new[] { answer0, answer1, answer2, answer3 };
            var question = new MultipleChoiceQuestion(text, answers, correctIndex);
            _questions.Add(question);
        }

        public bool CheckAnswer(int questionIndex, int selectedIndex)
        {
            if (questionIndex < 0 || questionIndex >= _questions.Count)
                throw new ArgumentOutOfRangeException(nameof(questionIndex));

            if (_questions[questionIndex] is MultipleChoiceQuestion mcq)
            {
                return mcq.IsCorrect(selectedIndex);
            }

            return false;
        }
    }
}
