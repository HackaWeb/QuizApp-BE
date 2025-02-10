using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.DataContext.Repositories;
public class FeedbackRepository(QuizAppDbContext context) : IFeedbackRepository
{
    private readonly QuizAppDbContext _context = context;

    public async Task<Feedback?> GetByIdAsync(Guid id)
    {
        return await _context.Feedback.FindAsync(id);
    }

    public async Task<List<Feedback>> GetByQuizIdAsync(Guid quizId)
    {
        return await _context.Feedback
            .Where(f => f.QuizId == quizId)
            .ToListAsync();
    }

    public async Task AddAsync(Feedback feedback)
    {
        feedback.CreatedAt = DateTime.UtcNow;
        await _context.Feedback.AddAsync(feedback);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Feedback feedback)
    {
        var existingFeedback = await _context.Feedback.FindAsync(feedback.Id);
        if (existingFeedback == null)
        {
            throw new KeyNotFoundException($"Feedback with ID {feedback.Id} not found.");
        }

        _context.Entry(existingFeedback).CurrentValues.SetValues(feedback);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var feedback = await _context.Feedback.FindAsync(id);
        if (feedback == null)
        {
            throw new KeyNotFoundException($"Feedback with ID {id} not found.");
        }

        _context.Feedback.Remove(feedback);
        await _context.SaveChangesAsync();
    }
}
