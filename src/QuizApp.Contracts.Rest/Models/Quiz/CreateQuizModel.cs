using Microsoft.AspNetCore.Http;

namespace QuizApp.Contracts.Rest.Models.Quiz;

public class CreateQuizModel
{
    public string Title { get; set; }

    public string Description { get; set; }

    public uint Duration { get; set; }

    public IFormFile? File { get; set; }


    public List<CreateQuestionModel> Questions { get; set; }
}
