namespace QuizApp.Contracts.Rest.Models.Quiz;

public class CreateQuestionModel
{
    public string Title { get; set; }
    public QuestionType Type { get; set; }

    public List<OptionModel>? Options { get; set; }
}