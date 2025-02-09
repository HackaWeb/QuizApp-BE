using MediatR;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public record GetQuizzesByUserRequest(string? UserId, int PageNumber, int PageSize) : IRequest<GetQuizzesByUserResponse>;
