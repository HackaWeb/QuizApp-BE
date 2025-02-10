using MediatR;

namespace QuizApp.Contracts.Rest.Requests;
public record GetFeedbackByIdRequest(Guid FeedbackId) : IRequest<GetFeedbackByIdResponse>;

public record GetFeedbackByIdResponse(Guid FeedbackId, Guid QuizId, string Text, ushort Rate, DateTime CreatedAt);

