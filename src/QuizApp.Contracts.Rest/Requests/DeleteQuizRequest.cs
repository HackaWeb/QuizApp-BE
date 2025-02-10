using MediatR;

namespace QuizApp.Contracts.Rest.Requests;

public record DeleteQuizRequest(Guid quizId) : IRequest;
