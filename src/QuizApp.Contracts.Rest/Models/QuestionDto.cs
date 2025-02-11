using MediatR;

namespace QuizApp.Contracts.Rest.Models;

public class QuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public Guid QuizId { get; set; }
}

public class UpdateQuestionModel
{
    public string? Text { get; set; }
    public QuestionType? Type { get; set; }
}

public class OverwriteQuestionsDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public QuestionType Type { get; set; }
    public Guid QuizId { get; set; }

    public List<OverwriteOptionsDto> Options { get; set; }
}

public class QuestionWithOptions
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public Guid QuizId { get; set; }
    List<AnonymousOptionDto> Options { get; set;}
}

public class AnonymousOptionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid QuestionId { get; set; }
}

public record CreateQuestionRequest(Guid QuizId, Guid QuestionId, string Text, QuestionType Type) : IRequest<QuestionDto>;

public record UpdateQuestionRequest(Guid QuestionId, string? Text, QuestionType? Type) : IRequest<QuestionDto>;

public record GetQuestionRequest(Guid QuestionId) : IRequest<QuestionDto>;

public record DeleteQuestionRequest(Guid QuestionId) : IRequest;

public record OverwriteQuestionsRequest(Guid quizId, List<OverwriteQuestionsDto> questions) : IRequest;

public record GetQuizQuestions(Guid quizId) : IRequest<List<QuestionWithOptions>>;