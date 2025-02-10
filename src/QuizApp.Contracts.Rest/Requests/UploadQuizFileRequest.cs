using MediatR;
using Microsoft.AspNetCore.Http;

namespace QuizApp.Contracts.Rest.Requests;
public class UploadQuizFileRequest : IRequest
{
    public IFormFile File { get; set; }
    public string QuizId { get; set; }
}
