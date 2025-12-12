using System;
using System.Collections.Generic;

namespace App.Domain.Entities
{
    public abstract class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Text { get; set; } = string.Empty;

        public int Order { get; set; }

        public Guid QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        // N-N связь через Tag
        public List<Tag> Tags { get; set; } = new();

        // Добавляем AnswerOptions сюда, чтобы все наследники имели варианты ответов
        public List<AnswerOption> AnswerOptions { get; set; } = new();
    }
}
