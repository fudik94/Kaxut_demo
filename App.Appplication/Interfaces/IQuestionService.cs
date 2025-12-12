using App.Application.DTO;

namespace App.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionDto?> GetByIdAsync(Guid id);
        Task<List<QuestionDto>> GetByQuizIdAsync(Guid quizId);
        Task<QuestionDto> CreateAsync(Guid quizId, QuestionDto question);
        Task<bool> DeleteAsync(Guid id);
    }
}
