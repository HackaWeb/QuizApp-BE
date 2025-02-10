using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.DataContext.Repositories;


public class QuestionRepository(QuizAppDbContext context) : IQuestionRepository
{
    private readonly QuizAppDbContext _context = context;
    public async Task<Question?> GetByIdAsync(Guid id)
    {
        return await _context.Questions
            .Include(q => q.ChoiceOptions)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<List<Question>> GetByQuizIdAsync(Guid quizId)
    {
        return await _context.Questions
            .Where(q => q.QuizId == quizId)
            .Include(q => q.ChoiceOptions)
            .ToListAsync();
    }

    public async Task<List<Question>> GetByIdsAsync(List<Guid> ids)
    {
        return await _context.Questions
            .Where(q => ids.Contains(q.Id))
            .Include(q => q.ChoiceOptions)
            .ToListAsync();
    }

    public async Task AddAsync(Question question)
    {
        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<Question> questions)
    {
        await _context.Questions.AddRangeAsync(questions);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Question question)
    {
        var existingQuestion = await _context.Questions
            .Include(q => q.ChoiceOptions)
            .FirstOrDefaultAsync(q => q.Id == question.Id);

        if (existingQuestion == null)
        {
            throw new KeyNotFoundException($"Question with ID {question.Id} not found.");
        }

        _context.Entry(existingQuestion).CurrentValues.SetValues(question);

        foreach (var option in question.ChoiceOptions)
        {
            var existingOption = existingQuestion.ChoiceOptions.FirstOrDefault(o => o.Id == option.Id);
            if (existingOption != null)
            {
                _context.Entry(existingOption).CurrentValues.SetValues(option);
            }
            else
            {
                existingQuestion.ChoiceOptions.Add(option);
            }
        }

        existingQuestion.ChoiceOptions.RemoveAll(o => !question.ChoiceOptions.Any(oo => oo.Id == o.Id));

        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<Question> questions)
    {
        foreach (var question in questions)
        {
            await UpdateAsync(question);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question == null)
        {
            throw new KeyNotFoundException($"Question with ID {id} not found.");
        }

        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(List<Guid> ids)
    {
        var questions = await _context.Questions
            .Where(q => ids.Contains(q.Id))
            .ToListAsync();

        _context.Questions.RemoveRange(questions);
        await _context.SaveChangesAsync();
    }
}
