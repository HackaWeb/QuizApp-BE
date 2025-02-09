using MediatR;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public record GetUserProfileRequest(string UserId) : IRequest<GetUserProfileResponse>;
