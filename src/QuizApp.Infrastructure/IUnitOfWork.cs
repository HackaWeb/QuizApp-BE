namespace QuizApp.Infrastructure;

public interface IUnitOfWork
{
    Task SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
