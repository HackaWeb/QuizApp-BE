namespace QuizApp.Domain.Models;

public class QuizHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
}
