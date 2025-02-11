using QuizApp.Domain.Models;

namespace QuizApp.Infrastructure.Repositories;

public interface IQuizHistoryRepository
{
    Task<QuizHistory?> GetByIdAsync(Guid historyId);
    Task<List<QuizHistory>> GetByQuizIdAsync(Guid quizId);
    Task<List<QuizHistory>> GetByUserIdAsync(Guid userId);
    Task AddAsync(QuizHistory history);
    Task UpdateAsync(QuizHistory history);
    Task DeleteAsync(Guid historyId);
}

