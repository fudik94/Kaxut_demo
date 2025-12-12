using App.Application.DTO;
using App.Application.Interfaces;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Services
{

    public class QuizService : IQuizService
    {
        private readonly AppDbContext _db;

        public QuizService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<QuizDto>> GetAllQuizzesAsync()
        {
            var quizzes = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();

            return quizzes.Select(q => new QuizDto
            {
                Id = q.Id,
                Title = q.Title,
                Questions = q.Questions.Select(qn => new QuestionDto
                {
                    Id = qn.Id,
                    Text = qn.Text,
                    Order = qn.Order,
                    AnswerOptions = qn.AnswerOptions.Select(ao => new AnswerOptionDto
                    {
                        Id = ao.Id,
                        Text = ao.Text,
                        IsCorrect = ao.IsCorrect
                    }).ToList()
                }).ToList()
            }).ToList();
        }

        public async Task<QuizDto?> GetQuizByIdAsync(Guid id)
        {
            var quiz = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null) return null;

            return new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Questions = quiz.Questions.Select(qn => new QuestionDto
                {
                    Id = qn.Id,
                    Text = qn.Text,
                    Order = qn.Order,
                    AnswerOptions = qn.AnswerOptions.Select(ao => new AnswerOptionDto
                    {
                        Id = ao.Id,
                        Text = ao.Text,
                        IsCorrect = ao.IsCorrect
                    }).ToList()
                }).ToList()
            };
        }
    }
}