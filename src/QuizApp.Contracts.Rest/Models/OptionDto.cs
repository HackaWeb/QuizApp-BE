using MediatR;

namespace QuizApp.Contracts.Rest.Models;
public class OptionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }
}

public class UpdateOptionModel
{
    public string? Title { get; set; }
    public bool? IsCorrect { get; set; }
}

public record CreateOptionRequest(Guid QuestionId, string Title, bool IsCorrect) : IRequest<OptionDto>;
public record UpdateOptionRequest(Guid OptionId, string? Title, bool? IsCorrect) : IRequest<OptionDto>;
public record GetOptionRequest(Guid OptionId) : IRequest<OptionDto>;
public record DeleteOptionRequest(Guid OptionId) : IRequest;

