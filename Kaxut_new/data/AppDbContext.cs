using Kaxut_new.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Kaxut_new.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }
        public DbSet<ChoiceQuestion> ChoiceQuestions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ------------------------------
            // Настройка наследования (TPH)
            // ------------------------------
            modelBuilder.Entity<Question>()
                .HasDiscriminator<string>("QuestionType")
                .HasValue<MultipleChoiceQuestion>("MultipleChoice")
                .HasValue<ChoiceQuestion>("Choice");

            // ------------------------------
            // Уникальный бизнес-индекс
            // ------------------------------
            modelBuilder.Entity<Quiz>()
                .HasIndex(q => q.Code)
                .IsUnique();

            // ------------------------------
            // N-N связь Question <-> Tag
            // ------------------------------
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Tags)
                .WithMany(t => t.Questions);

            // ------------------------------
            // Seed данных
            // ------------------------------
            var quiz1Id = Guid.NewGuid();
            var q1Id = Guid.NewGuid();
            var q2Id = Guid.NewGuid();

            modelBuilder.Entity<Quiz>().HasData(new Quiz
            {
                Id = quiz1Id,
                Code = "QZ001",
                Title = "C# Basics",
                CreatedAt = DateTime.UtcNow
            });

            modelBuilder.Entity<MultipleChoiceQuestion>().HasData(
                new
                {
                    Id = q1Id,
                    QuizId = quiz1Id,
                    Text = "Что такое переменная?",
                    Order = 1,
                    CorrectIndex = 1,
                    QuestionType = "MultipleChoice"
                },
                new
                {
                    Id = q2Id,
                    QuizId = quiz1Id,
                    Text = "Что такое цикл?",
                    Order = 2,
                    CorrectIndex = 0,
                    QuestionType = "MultipleChoice"
                }
            );

            modelBuilder.Entity<AnswerOption>().HasData(
                new
                {
                    Id = Guid.NewGuid(),
                    Text = "Место для хранения значения",
                    IsCorrect = true,
                    QuestionId = q1Id
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Text = "Тип данных",
                    IsCorrect = false,
                    QuestionId = q1Id
                },

                new
                {
                    Id = Guid.NewGuid(),
                    Text = "Повторяющееся выполнение блока кода",
                    IsCorrect = true,
                    QuestionId = q2Id
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Text = "Класс в C#",
                    IsCorrect = false,
                    QuestionId = q2Id
                }
            );
        }
    }
}
