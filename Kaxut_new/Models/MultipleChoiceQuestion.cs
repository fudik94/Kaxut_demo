using System;

namespace Kaxut_new.Models
{
    public class MultipleChoiceQuestion : Question
    {
        public string[] Answers { get; set; } = new string[4];
        public int CorrectIndex { get; set; }

        public MultipleChoiceQuestion(string text, string[] answers, int correctIndex)
        {
            Text = text;
            Answers = answers;
            CorrectIndex = correctIndex;
        }

        public bool IsCorrect(int selectedIndex) => selectedIndex == CorrectIndex;
    }
}
