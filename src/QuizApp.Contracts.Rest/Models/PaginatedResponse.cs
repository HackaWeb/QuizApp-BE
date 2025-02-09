namespace QuizApp.Contracts.Rest.Models;

public class PaginatedResponse<T>
{
    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();
}
