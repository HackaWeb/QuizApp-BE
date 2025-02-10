using MediatR;
using QuizApp.Contracts.Rest.Models;

namespace QuizApp.Contracts.Rest.Requests;

public record DeleteUserProfileRequest(string UserId) : IRequest;
