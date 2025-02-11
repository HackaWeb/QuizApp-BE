using MediatR;
using QuizApp.Contracts.Rest.Models.Quiz;

namespace QuizApp.Contracts.Rest.Requests;

public record DeleteUserImageCommand(string UserId) : IRequest;

public record GetUserCompletedQuests(string UserId) : IRequest<List<QuizModel>>;
