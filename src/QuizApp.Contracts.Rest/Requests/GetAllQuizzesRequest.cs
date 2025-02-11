using MediatR;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public record GetAllQuizzesRequest(SortType sortType, string? titleFilter = null) : IRequest<GetAllQuizzesResponse>;

public enum SortType
{
    Rating,
    NumberOfPasses,
    Alphabet,
    AuthorRating,
}
