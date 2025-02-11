using MediatR;

namespace QuizApp.Contracts.Rest.Requests;

public class QuizDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public uint Duration { get; set; }
}

public record UpdateQuizRequest(Guid QuizId, string? Title, string? Description, uint? Duration) : IRequest<QuizDto>;

public record GetQuizRequest(Guid QuizId) : IRequest<QuizDto>;

public class UpdateQuizModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public uint? Duration { get; set; }
}