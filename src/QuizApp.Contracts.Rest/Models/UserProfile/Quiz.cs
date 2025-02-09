namespace QuizApp.Contracts.Rest.Models.UserProfile;

public record Quiz(
    string Title, 
    string? ImageUrl, 
    ushort Duration, 
    int PlayedTimes,
    ushort? Rate,
    ushort TaskCount);
