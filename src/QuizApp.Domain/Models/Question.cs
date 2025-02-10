namespace QuizApp.Domain.Models;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Text { get; set; }

    public string? MediaUrl { get; set; }

    public QuestionType Type { get; set; }

    public List<AnswerOption>? ChoiceOptions { get; set; }

    public Quiz Quiz { get; set; }
    public Guid QuizId { get; set; }
}
