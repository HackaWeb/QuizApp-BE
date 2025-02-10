using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure.Repositories;
using System;

namespace QuizApp.DataContext.Repositories;

public class QuizRepository(QuizAppDbContext _context) : IQuizRepository
{
    public async Task<Quiz?> GetByIdAsync(Guid id)
    {
        return await _context.Quizzes
            .Include(q => q.Questions)
            .ThenInclude(q => q.ChoiceOptions)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<List<Quiz>> GetAllAsync()
    {
        return await _context.Quizzes
            .Include(q => q.Questions)
            .ThenInclude(q => q.ChoiceOptions)
            .ToListAsync();
    }

    public async Task AddAsync(Quiz quiz)
    {
        _context.Quizzes.Add(quiz);
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        var existingQuiz = await _context.Quizzes
            .Include(q => q.Questions)
            .ThenInclude(q => q.ChoiceOptions)
            .FirstOrDefaultAsync(q => q.Id == quiz.Id);

        if (existingQuiz == null)
        {
            throw new KeyNotFoundException($"Quiz with ID {quiz.Id} not found.");
        }

        _context.Entry(existingQuiz).CurrentValues.SetValues(quiz);

        foreach (var question in quiz.Questions)
        {
            var existingQuestion = existingQuiz.Questions.FirstOrDefault(q => q.Id == question.Id);
            if (existingQuestion != null)
            {
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
            }
            else
            {
                existingQuiz.Questions.Add(question);
            }
        }

        existingQuiz.Questions.RemoveAll(q => !quiz.Questions.Any(qn => qn.Id == q.Id));

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var quiz = await _context.Quizzes.FindAsync(id);
        if (quiz == null)
        {
            throw new KeyNotFoundException($"Quiz with ID {id} not found.");
        }

        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();
    }
}
