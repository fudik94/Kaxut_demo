using System;
using System.Collections.Generic;

namespace Kaxut_new.Models
{
    public class ChoiceQuestion : Question
    {
        public bool Multiple { get; set; } = false;  // можно выбрать один или несколько ответов

        public List<string> Options { get; set; } = new(); // список вариантов ответа
    }
}
