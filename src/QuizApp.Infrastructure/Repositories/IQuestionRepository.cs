using QuizApp.Domain.Models;

namespace QuizApp.Infrastructure.Repositories;

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(Guid id);
    Task<List<Question>> GetByQuizIdAsync(Guid quizId);
    Task<List<Question>> GetByIdsAsync(List<Guid> ids);

    Task AddAsync(Question question);
    Task AddRangeAsync(List<Question> questions);

    Task UpdateAsync(Question question);
    Task UpdateRangeAsync(List<Question> questions);

    Task DeleteAsync(Guid id);
    Task DeleteRangeAsync(List<Guid> ids);
}

