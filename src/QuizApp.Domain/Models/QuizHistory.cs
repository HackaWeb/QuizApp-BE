namespace QuizApp.Domain.Models;

public class QuizHistory
{
    public Guid Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }
    public Guid UserId { get; set; }
    public Quiz Quiz { get; set; }
}
