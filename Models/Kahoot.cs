using System;
using System.Collections.ObjectModel;

namespace Kaxut_demo
{
    /// <summary>
    /// Represents a Kahoot quiz containing a list of questions.
    /// </summary>
    public sealed class Kahoot
    {
        // Внутренняя коллекция вопросов (ObservableCollection для автообновления UI)
        private readonly ObservableCollection<Question> _questions = new ObservableCollection<Question>();

        // Публичный доступ
        public ObservableCollection<Question> Questions => _questions;

        /// <summary>
        /// Adds an existing question instance.
        /// </summary>
        /// <param name="question">A valid Question instance.</param>
        public void AddQuestion(Question question)
        {
            // Добавление готового объекта вопроса
            if (question == null) throw new ArgumentNullException(nameof(question));
            _questions.Add(question);
        }

        /// <summary>
        /// Adds a new question given raw values.
        /// </summary>
        /// <param name="text">Question text.</param>
        /// <param name="answers">Array of exactly 4 answer options.</param>
        /// <param name="correctIndex">Index of the correct option (0..3).</param>
        public void AddQuestion(string text, string[] answers, int correctIndex)
        {
            // Удобный способ добавить вопрос из значений
            var question = new Question(text, answers, correctIndex);
            _questions.Add(question);
        }

        /// <summary>
        /// Adds a new question given raw values.
        /// Convenience overload with four separate answer strings.
        /// </summary>
        /// <param name="text">Question text.</param>
        /// <param name="answer0">Answer option 0.</param>
        /// <param name="answer1">Answer option 1.</param>
        /// <param name="answer2">Answer option 2.</param>
        /// <param name="answer3">Answer option 3.</param>
        /// <param name="correctIndex">Index of the correct option (0..3).</param>
        public void AddQuestion(string text, string answer0, string answer1, string answer2, string answer3, int correctIndex)
        {
            var answers = new[] { answer0, answer1, answer2, answer3 };
            var question = new Question(text, answers, correctIndex);
            _questions.Add(question);
        }

        /// <summary>
        /// Checks if the selected answer for a specific question is correct.
        /// </summary>
        /// <param name="questionIndex">Index of the question in the quiz.</param>
        /// <param name="selectedIndex">Selected answer index (0..3).</param>
        /// <returns>True if correct; otherwise, false.</returns>
        public bool CheckAnswer(int questionIndex, int selectedIndex)
        {
            // Проверяем границы индекса вопроса
            if (questionIndex < 0 || questionIndex >= _questions.Count)
                throw new ArgumentOutOfRangeException(nameof(questionIndex), "Question index is out of range.");

            // Делегируем проверку самому вопросу
            return _questions[questionIndex].IsCorrect(selectedIndex);
        }
    }
}