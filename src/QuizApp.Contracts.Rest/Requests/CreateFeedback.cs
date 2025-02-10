using MediatR;

namespace QuizApp.Contracts.Rest.Requests;

public record CreateFeedbackRequest(Guid QuizId, string Text, ushort Rate) : IRequest<CreateFeedbackResponse>;

public record CreateFeedbackResponse(Guid FeedbackId, Guid QuizId, string Text, ushort Rate, DateTime CreatedAt);

