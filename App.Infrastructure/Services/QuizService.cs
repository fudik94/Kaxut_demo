using App.Application.DTO;
using App.Application.Interfaces;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services
{
    public class QuizService : IQuizService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<QuizService> _logger;

        public QuizService(
            AppDbContext db,
            ILogger<QuizService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<QuizDto>> GetAllQuizzesAsync()
        {
            _logger.LogInformation("Loading all quizzes");

            var quizzes = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();

            _logger.LogInformation(
                "Loaded {Count} quizzes", quizzes.Count);

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
            _logger.LogInformation(
                "Loading quiz with Id: {QuizId}", id);

            var quiz = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                _logger.LogWarning(
                    "Quiz with Id {QuizId} not found", id);
                return null;
            }

            _logger.LogInformation(
                "Quiz with Id {QuizId} loaded successfully", id);

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
