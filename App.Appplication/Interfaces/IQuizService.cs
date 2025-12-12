using App.Application.DTO;
namespace App.Application.Interfaces
{

    public interface IQuizService
    {
        Task<List<QuizDto>> GetAllQuizzesAsync();
        Task<QuizDto?> GetQuizByIdAsync(Guid id);
    }
}