using Microsoft.AspNetCore.Http;

namespace QuizApp.Contracts.Rest.Requests;

public class UpdateUserProfileRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public IFormFile? Avatar { get; init; }
}
