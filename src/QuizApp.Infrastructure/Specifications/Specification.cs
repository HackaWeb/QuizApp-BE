using System.Linq.Expressions;

namespace QuizApp.Infrastructure.Specifications;
public abstract class Specification<TEntity>(Expression<Func<TEntity, bool>>? criteria, bool isReadOnly = false)
    where TEntity : class
{
    /// <summary>
    /// Gets the expression representing the query criteria for filtering entities.
    /// </summary>
    public Expression<Func<TEntity, bool>>? Criteria { get; } = criteria;

    /// <summary>
    /// Gets the list of expressions used to specify navigation properties to include in the query.
    /// </summary>
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    /// <summary>
    /// Gets the expression used to specify the property for ascending ordering of query results.
    /// </summary>
    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

    /// <summary>
    /// Gets the expression used to specify the property for descending ordering of query results.
    /// </summary>
    public Expression<Func<TEntity, object>>? OrderDescByExpression { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the query should be executed in read-only mode.
    /// </summary>
    public bool IsReadOnly { get; } = isReadOnly;

    /// <summary>
    /// Adds a navigation property to include in the query results.
    /// </summary>
    /// <param name="includeExpression">
    /// An expression specifying the navigation property to include.
    /// </param>
    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) =>
        IncludeExpressions.Add(includeExpression);

    /// <summary>
    /// Sets the property for ascending ordering of query results.
    /// </summary>
    /// <param name="orderByExpression">
    /// An expression specifying the property to use for ordering in ascending order.
    /// </param>
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) =>
        OrderByExpression = orderByExpression;

    /// <summary>
    /// Sets the property for descending ordering of query results.
    /// </summary>
    /// <param name="orderByDescExpression">
    /// An expression specifying the property to use for ordering in descending order.
    /// </param>
    protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression) =>
        OrderDescByExpression = orderByDescExpression;
}
