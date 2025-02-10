using MediatR;

namespace QuizApp.Contracts.Rest.Requests;

public record UpdateFeedbackRequest(Guid FeedbackId, string Text, ushort Rate) : IRequest;

