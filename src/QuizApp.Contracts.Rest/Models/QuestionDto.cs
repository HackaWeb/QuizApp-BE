using MediatR;

namespace QuizApp.Contracts.Rest.Models;

public class QuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public Guid QuizId { get; set; }
}

public record CreateQuestionRequest(Guid QuizId, Guid QuestionId, string Text, QuestionType Type) : IRequest<QuestionDto>;

public record UpdateQuestionRequest(Guid QuestionId, string? Text, QuestionType? Type) : IRequest<QuestionDto>;

public record GetQuestionRequest(Guid QuestionId) : IRequest<QuestionDto>;

public record DeleteQuestionRequest(Guid QuestionId) : IRequest;

public class UpdateQuestionModel
{
    public string? Text { get; set; }
    public QuestionType? Type { get; set; }
}