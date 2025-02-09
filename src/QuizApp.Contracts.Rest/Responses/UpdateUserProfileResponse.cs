namespace QuizApp.Contracts.Rest.Responses;

public record UpdateUserProfileResponse(
    string userId,
    string email,
    string firstName,
    string lastName,
    string? avatarUrl);

