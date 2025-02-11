using MediatR;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public record GetQuizzesByUserIdCommand(Guid UserId) : IRequest<GetQuizzesByUserResponse>;

public record GetQuizWithoutQuestionsCommand(Guid quizId) : IRequest<QuizModel>;
