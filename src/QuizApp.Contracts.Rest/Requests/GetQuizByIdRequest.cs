using MediatR;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public record GetQuizByIdRequest(Guid id) : IRequest<GetQuizByIdResponse>;
