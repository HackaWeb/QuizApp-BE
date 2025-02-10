using MediatR;

namespace QuizApp.Contracts.Rest.Requests;
public record DeleteFeedbackRequest(Guid FeedbackId) : IRequest;