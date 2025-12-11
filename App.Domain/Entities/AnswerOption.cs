using System;

namespace App.Domain.Entities
{
    public class AnswerOption
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public Guid QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
