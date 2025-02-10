namespace QuizApp.Contracts.Rest.Models.Quiz;
public class FeedbackModel
{
    public Guid Id { get; set; }

    public string Text { get; set; }

    public ushort Rate { get; set; }

    public DateTime CreatedAt { get; set; }
}
