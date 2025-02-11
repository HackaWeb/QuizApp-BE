using MediatR;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;
public record GetAllQuizzesRequest(SortType sortType) : IRequest<GetAllQuizzesResponse>;


public enum SortType
{
    Rating,
    NumberOfPasses,
    Alphabet,
    AuthorRating,
}
