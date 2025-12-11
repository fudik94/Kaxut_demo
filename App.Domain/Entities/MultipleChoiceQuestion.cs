using System.Collections.Generic;

namespace App.Domain.Entities
{
    public class MultipleChoiceQuestion : Question
    {
        public List<AnswerOption> Options { get; set; } = new();

        public int CorrectIndex { get; set; }
    }
}
