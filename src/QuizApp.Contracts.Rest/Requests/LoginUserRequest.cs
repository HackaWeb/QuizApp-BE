using MediatR;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public record LoginUserRequest(string Email, string Password) : IRequest<TokenResponse>;