namespace QuizApp.Contracts.Rest.Models.Quiz;

public class OptionModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsCorrect { get; set; }
}
