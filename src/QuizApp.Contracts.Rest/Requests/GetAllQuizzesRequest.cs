using MediatR;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;
public record GetAllQuizzesRequest() : IRequest<GetAllQuizzesResponse>;
