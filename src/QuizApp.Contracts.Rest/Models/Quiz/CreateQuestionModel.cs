using Microsoft.AspNetCore.Http;

namespace QuizApp.Contracts.Rest.Models.Quiz;

public class CreateQuestionModel
{
    public string Title { get; set; }
    public QuestionType Type { get; set; }
    public IFormFile? File { get; set; }

    public List<OptionModel>? Options { get; set; }
}