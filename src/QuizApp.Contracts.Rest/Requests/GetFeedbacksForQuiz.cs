using MediatR;

namespace QuizApp.Contracts.Rest.Requests;

public record GetFeedbacksForQuizRequest(Guid QuizId) : IRequest<List<GetFeedbackByIdResponse>>;

