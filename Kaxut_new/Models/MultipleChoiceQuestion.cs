using System.Collections.Generic;

namespace Kaxut_new.Models
{
    public class MultipleChoiceQuestion : Question
    {
        public List<AnswerOption> Options { get; set; } = new();

        public int CorrectIndex { get; set; }
    }
}
