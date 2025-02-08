namespace QuizApp.Domain.Models;

public class Quiz
{
    private Quiz()
    {
    }

    public Quiz(string description)
    {
        Description = description;
    }

    public Guid Id { get; private init; }

    public string Description { get; private init; }

    public DateTime CreatedAt { get; private init; }
}
