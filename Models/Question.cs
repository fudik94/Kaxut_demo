using System;

namespace Kaxut_demo
{
    /// <summary>
    /// Represents a single quiz question with four answer options.
    /// </summary>
    public sealed class Question
    {
        public string Text { get; }

        public string[] Answers
        {
            get
            {
                var copy = new string[_answers.Length];
                Array.Copy(_answers, copy, _answers.Length);
                return copy;
            }
        }

        public int CorrectIndex { get; }

        private readonly string[] _answers;

        public Question(string text, string[] answers, int correctIndex)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Question text cannot be null or empty.", nameof(text));

            if (answers == null)
                throw new ArgumentNullException(nameof(answers));

            if (answers.Length != 4)
                throw new ArgumentException("Answers array must contain exactly 4 options.", nameof(answers));

            for (int i = 0; i < answers.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(answers[i]))
                    throw new ArgumentException($"Answer at index {i} cannot be null or empty.", nameof(answers));
            }

            if (correctIndex < 0 || correctIndex > 3)
                throw new ArgumentOutOfRangeException(nameof(correctIndex), "Correct index must be between 0 and 3.");

            Text = text;
            _answers = new string[4];
            Array.Copy(answers, _answers, 4);
            CorrectIndex = correctIndex;
        }

        public bool IsCorrect(int selectedIndex)
        {
            if (selectedIndex < 0 || selectedIndex > 3)
                return false;

            return selectedIndex == CorrectIndex;
        }

        public override string ToString() => Text;
    }
}