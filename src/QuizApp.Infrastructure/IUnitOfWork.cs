using QuizApp.Domain.Models;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.Infrastructure;

public interface IUnitOfWork
{
    IQuestionRepository QuestionRepository { get; }

    IOptionRepository OptionsRepository { get;  }

    IRepository<QuizHistory> QuizHistoryRepository { get; }

    IQuizRepository QuizRepository { get; }
    
    IFeedbackRepository FeedbackRepository { get; }

    Task SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
