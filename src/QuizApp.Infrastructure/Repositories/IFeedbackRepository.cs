using QuizApp.Domain.Models;

namespace QuizApp.Infrastructure.Repositories;

public interface IFeedbackRepository
{
    Task<List<Feedback>> GetByQuizId(Guid quizId);
    Task<Feedback?> GetByIdAsync(Guid id);
    Task<List<Feedback>> GetByQuizIdAsync(Guid quizId);
    Task AddAsync(Feedback feedback);
    Task UpdateAsync(Feedback feedback);
    Task DeleteAsync(Guid id);
}
