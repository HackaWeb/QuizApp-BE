namespace QuizApp.Domain.Models;

public class Feedback
{
    public Guid Id { get; set; }

    public string Text { get; set; }

    public ushort Rate { get; set; }

    public DateTime CreatedAt { get; set; }

    public Quiz Quiz { get; set; }

    public Guid QuizId { get; set; }

    public Guid UserId { get; set; }
}
