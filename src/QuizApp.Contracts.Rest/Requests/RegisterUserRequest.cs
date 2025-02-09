using MediatR;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public record RegisterUserRequest(string Email, string Password) : IRequest<TokenResponse>;
