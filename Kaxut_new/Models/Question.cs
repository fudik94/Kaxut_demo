using System;
using System.Collections.Generic;

namespace Kaxut_new.Models
{
    public abstract class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public int Order { get; set; }
        public string Text { get; set; } = string.Empty;

        // Теги (опционально)
        public List<Tag> Tags { get; set; } = new();
    }
}
