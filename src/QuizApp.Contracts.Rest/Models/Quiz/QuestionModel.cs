namespace QuizApp.Contracts.Rest.Models.Quiz;

public class QuestionModel
{
    public Guid Id { get; set; }

    public string Text { get; set; }

    public string? MediaUrl { get; set; }

    public QuestionType Type { get; set; }

    public List<AnswerOptionModel>? ChoiceOptions { get; set; }
}
