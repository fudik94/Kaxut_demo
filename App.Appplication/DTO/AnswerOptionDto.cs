namespace App.Application.DTO
{

    public class AnswerOptionDto
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
