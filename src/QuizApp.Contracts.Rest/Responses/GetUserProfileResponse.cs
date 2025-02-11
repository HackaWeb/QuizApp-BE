namespace QuizApp.Contracts.Rest.Responses;

public record GetUserProfileResponse(
    string? FirstName,
    string? LastName,
    string Email,
    string? Avatar,
    bool IsAdmin
    );
