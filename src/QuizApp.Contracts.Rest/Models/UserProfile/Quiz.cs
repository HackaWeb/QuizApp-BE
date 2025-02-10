namespace QuizApp.Contracts.Rest.Models.UserProfile;

public record Quiz(
    Guid Id,
    string Title, 
    string? ImageUrl, 
    uint Duration, 
    int PlayedTimes,
    ushort? Rate,
    ushort TaskCount);
