using QuizApp.Domain.Models;

namespace QuizApp.Infrastructure.Repositories;

public interface IQuizRepository
{
    Task<Quiz?> GetByIdAsync(Guid id);
    Task<List<Quiz>> GetAllAsync();
    Task AddAsync(Quiz quiz);
    Task UpdateAsync(Quiz quiz);
    Task DeleteAsync(Guid id);
}

