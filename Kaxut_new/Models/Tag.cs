using System;
using System.Collections.Generic;

namespace Kaxut_new.Models
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public List<Question> Questions { get; set; } = new();
    }
}
