using QuizApp.Domain.Models;

namespace QuizApp.Infrastructure.Repositories;
public interface IOptionRepository
{
    Task<AnswerOption?> GetByIdAsync(Guid id);
    Task AddAsync(AnswerOption option);
    Task UpdateAsync(AnswerOption option);
    Task DeleteAsync(Guid id);
}
