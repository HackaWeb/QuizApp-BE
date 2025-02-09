namespace QuizApp.Domain.Models;

public class Quiz
{
    private Quiz()
    {
    }

    public Quiz(string description, string imageUrl, ushort duration)
    {
        Description = description;
        ImageUrl = imageUrl;
        Duration = duration;
    }

    public Guid Id { get; private init; }

    public string Title { get; private init; }

    public string Description { get; private init; }

    public DateTime CreatedAt { get; private init; }

    public string ImageUrl { get; private init; }

    public ushort? Rate { get; private init; }

    public uint PassCount { get; set; }

    public Guid OwnerId { get; set; }

    public ushort Duration { get; set; }

    public IReadOnlyList<Question> Questions { get; set; }

    public IReadOnlyList<Feedback> Feedbacks { get; set; }
}
