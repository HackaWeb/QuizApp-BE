namespace QuizApp.Domain.Models;

public class AnswerOption
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsCorrect { get; set; }

    public Question Question { get; set; }
    public Guid QuestionId { get; set; }
}
