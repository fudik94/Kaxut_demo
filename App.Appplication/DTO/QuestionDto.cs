namespace App.Application.DTO
{

    public class QuestionDto
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public int Order { get; set; }
        public List<AnswerOptionDto> AnswerOptions { get; set; } = new();
    }
}
