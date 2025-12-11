using System;
using System.Collections.Generic;

namespace Kaxut_new.Models
{
    public class Quiz
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Уникальное бизнес-значение
        public string Code { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 1-N: Quiz → Questions
        public List<Question> Questions { get; set; } = new();
    }
}
