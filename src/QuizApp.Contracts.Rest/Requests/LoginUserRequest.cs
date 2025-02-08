using MediatR;
using QuizApp.Contracts.Rest.Models;

namespace QuizApp.Contracts.Rest.Requests;

public record LoginUserRequest(string Email, string Password) : IRequest<Result>;