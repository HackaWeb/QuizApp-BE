using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.DataContext.Repositories;

public class QuizHistoryRepository(QuizAppDbContext context) : IQuizHistoryRepository
{
    private readonly QuizAppDbContext _context = context;

    public async Task<QuizHistory?> GetByIdAsync(Guid historyId)
    {
        return await _context.QuizHistory
            .Include(h => h.Quiz)
            .FirstOrDefaultAsync(h => h.Id == historyId);
    }

    public async Task<List<QuizHistory>> GetByQuizIdAsync(Guid quizId)
    {
        return await _context.QuizHistory
            .Include(h => h.Quiz)
            .Where(h => h.Quiz.Id == quizId)
            .ToListAsync();
    }

    public async Task<List<QuizHistory>> GetByUserIdAsync(Guid userId)
    {
        return await _context.QuizHistory
            .Where(h => h.UserId == userId)
            .Include(h => h.Quiz)
            .ToListAsync();
    }

    public async Task AddAsync(QuizHistory history)
    {
        _context.QuizHistory.Add(history);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(QuizHistory history)
    {
        _context.QuizHistory.Update(history);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid historyId)
    {
        var history = await GetByIdAsync(historyId);
        if (history != null)
        {
            _context.QuizHistory.Remove(history);
            await _context.SaveChangesAsync();
        }
    }
}
