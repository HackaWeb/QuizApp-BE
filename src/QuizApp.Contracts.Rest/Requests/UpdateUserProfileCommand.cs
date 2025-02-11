using MediatR;
using Microsoft.AspNetCore.Http;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public class UpdateUserProfileCommand : IRequest<UpdateUserProfileResponse>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserId { get; set; }
    public IFormFile? Avatar { get; set; }
}
