using QuizApp.Domain.Models;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.DataContext.Repositories;
public class OptionRepository : IOptionRepository
{
    private readonly QuizAppDbContext _context;

    public OptionRepository(QuizAppDbContext context)
    {
        _context = context;
    }

    public async Task<AnswerOption?> GetByIdAsync(Guid id)
    {
        return await _context.AnswerOptions.FindAsync(id);
    }

    public async Task AddAsync(AnswerOption option)
    {
        await _context.AnswerOptions.AddAsync(option);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AnswerOption option)
    {
        _context.AnswerOptions.Update(option);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var option = await _context.AnswerOptions.FindAsync(id);
        if (option != null)
        {
            _context.AnswerOptions.Remove(option);
            await _context.SaveChangesAsync();
        }
    }
}

