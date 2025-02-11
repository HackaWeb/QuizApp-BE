using MediatR;

namespace QuizApp.Contracts.Rest.Requests;

public record DeleteUserImageCommand(string UserId) : IRequest;
