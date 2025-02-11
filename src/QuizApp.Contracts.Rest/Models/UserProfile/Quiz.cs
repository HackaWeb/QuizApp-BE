namespace QuizApp.Contracts.Rest.Models.UserProfile;

public record Quiz(
    Guid Id,
    string Title, 
    string? ImageUrl, 
    uint Duration, 
    int TimesAttempted,
    ushort? Rate,
    ushort TaskCount);
