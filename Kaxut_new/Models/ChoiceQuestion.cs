using System.Collections.Generic;

namespace Kaxut_new.Models
{
    public class ChoiceQuestion : Question
    {
        public bool Multiple { get; set; }

        public List<AnswerOption> Options { get; set; } = new();
    }
}
