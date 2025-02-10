using MediatR;
using Microsoft.AspNetCore.Http;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public record UpdateUserProfileRequest( 
    string Email, 
    string FirstName, 
    string LastName) : IRequest<UpdateUserProfileResponse>
{
    public string? UserId { get; set; }
    public IFormFile? Avatar { get; init; }
}
