namespace App.Application.DTO
{

    public class QuizDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }
}