using MediatR;
using Microsoft.AspNetCore.Http;
using QuizApp.Contracts.Rest.Models;
using System.Text.Json.Serialization;

namespace QuizApp.Contracts.Rest.Requests;

public record UpdateUserProfileRequest(
    string UserId, 
    string Email, 
    string FirstName, 
    string LastName) : IRequest<Result>
{
    public IFormFile? Avatar { get; init; }
}
