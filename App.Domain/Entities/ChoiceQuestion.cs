using System.Collections.Generic;

namespace App.Domain.Entities
{
    public class ChoiceQuestion : Question
    {
        public bool Multiple { get; set; }

        public List<AnswerOption> Options { get; set; } = new();
    }
}
