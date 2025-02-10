using MediatR;
using Microsoft.AspNetCore.Http;

namespace QuizApp.Contracts.Rest.Requests;

public class UploadQuestionMediaRequest : IRequest
{
    public IFormFile File { get; set; }
    public string QuestionId { get; set; }
}
