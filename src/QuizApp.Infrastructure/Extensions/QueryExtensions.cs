using Microsoft.EntityFrameworkCore;
using QuizApp.Infrastructure.Specifications;

namespace QuizApp.Infrastructure.Extensions;
public static class QueryExtensions
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        this IQueryable<TEntity> input,
        Specification<TEntity> specification)
        where TEntity : class
    {
        var queryable = input;

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        queryable = specification.IncludeExpressions.Aggregate(
            queryable,
            (current, includeExpressions) => current.Include(includeExpressions));

        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderDescByExpression is not null)
        {
            queryable = queryable.OrderByDescending(specification.OrderDescByExpression);
        }

        if (specification.IsReadOnly)
        {
            queryable = queryable.AsNoTracking();
        }

        return queryable;
    }
}
