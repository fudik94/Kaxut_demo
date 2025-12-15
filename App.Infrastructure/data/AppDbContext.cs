using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using App.Application;

namespace App.Infrastructure.Data
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
            // Наследование TPH
            // ------------------------------
            modelBuilder.Entity<Question>()
                .HasDiscriminator<string>("QuestionType")
                .HasValue<MultipleChoiceQuestion>("MultipleChoice")
                .HasValue<ChoiceQuestion>("Choice");

            // ------------------------------
            // Уникальный индекс
            // ------------------------------
            modelBuilder.Entity<Quiz>()
                .HasIndex(q => q.Code)
                .IsUnique();

            // ------------------------------
            // Связь Many-to-Many Question <-> Tag
            // ------------------------------
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Tags)
                .WithMany(t => t.Questions);
        }
    }
}
