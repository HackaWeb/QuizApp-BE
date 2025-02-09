using MediatR;
using Microsoft.AspNetCore.Http;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public record UpdateUserProfileRequest(
    string userId, 
    string email, 
    string firstName, 
    string lastName) : IRequest<UpdateUserProfileResponse>
{
    public IFormFile? Avatar { get; init; }
}
