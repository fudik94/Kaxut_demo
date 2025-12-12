using System;
using System.Collections.ObjectModel;   
using System.Collections.Generic;

namespace App.Domain.Entities
{
    public class Quiz
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public ObservableCollection<Question> Questions { get; set; }
            = new ObservableCollection<Question>();
    }
}
