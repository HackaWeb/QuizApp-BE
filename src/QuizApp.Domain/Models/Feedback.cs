namespace QuizApp.Domain.Models;

public class Feedback
{
    private Feedback()
    {
    }

    public Feedback(string text, ushort rate)
    {
        Text = text;
        Rate = rate;
    }

    public Guid Id { get; set; }

    public string Text { get; set; }

    public ushort Rate { get; set; }

    public DateTime CreatedAt { get; set; }
}
