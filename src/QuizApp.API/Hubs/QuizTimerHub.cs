using Microsoft.AspNetCore.SignalR;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuizApp.API.Hubs;

public class QuizTimerHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;
    private static readonly ConcurrentDictionary<string, (DateTime StartTime, int Duration, CancellationTokenSource TokenSource)> ActiveQuizzes = new();

    public QuizTimerHub(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task StartQuiz(string quizId)
    {
        var quiz = await _unitOfWork.QuizRepository.GetByIdAsync(Guid.Parse(quizId));
        if (quiz == null)
        {
            await Clients.Caller.SendAsync("QuizNotFound", quizId);
            return;
        }

        int duration = (int)quiz.Duration;
        var startTime = DateTime.UtcNow;
        var tokenSource = new CancellationTokenSource();

        ActiveQuizzes[quizId] = (startTime, duration, tokenSource);

        _ = StopQuizAfterDelay(quizId, duration, tokenSource.Token);

        await Clients.All.SendAsync("QuizStarted", quizId, startTime, duration);
    }

    public async Task GetQuizTime(string quizId)
    {
        if (ActiveQuizzes.TryGetValue(quizId, out var quizData))
        {
            var elapsed = (int)(DateTime.UtcNow - quizData.StartTime).TotalSeconds;
            var remainingTime = Math.Max(quizData.Duration - elapsed, 0);

            await Clients.Caller.SendAsync("UpdateQuizTime", quizId, remainingTime);
        }
        else
        {
            await Clients.Caller.SendAsync("QuizNotFound", quizId);
        }
    }

    public async Task SubmitAnswers(string quizId, Dictionary<Guid, Guid> userAnswers)
    {
        if (!ActiveQuizzes.TryGetValue(quizId, out var quizData))
        {
            await Clients.Caller.SendAsync("QuizNotFound", quizId);
            return;
        }

        var quiz = await _unitOfWork.QuizRepository.GetByIdAsync(Guid.Parse(quizId));
        if (quiz == null)
        {
            await Clients.Caller.SendAsync("QuizNotFound", quizId);
            return;
        }

        int correctAnswers = 0;
        int totalQuestions = quiz.Questions.Count;

        foreach (var question in quiz.Questions)
        {
            if (userAnswers.TryGetValue(question.Id, out var userAnswer))
            {
                var correctOption = question.ChoiceOptions.FirstOrDefault(o => o.IsCorrect);
                if (correctOption != null && correctOption.Id == userAnswer)
                {
                    correctAnswers++;
                }
            }
        }

        double score = ((double)correctAnswers / totalQuestions) * 100;

        var quizHistory = new QuizHistory
        {
            Id = Guid.NewGuid(),
            FinishedAt = DateTime.Now,
            QuizId = Guid.Parse(quizId),
            StartedAt = DateTime.Now.AddMinutes(quiz.Duration),

        };

        await _unitOfWork.QuizHistoryRepository.AddAsync(quizHistory);

        await Clients.Caller.SendAsync("QuizCompleted", quizId, correctAnswers, totalQuestions, score);

        ActiveQuizzes.Remove(quizId, out _);
    }

    private async Task StopQuizAfterDelay(string quizId, int duration, CancellationToken token)
    {
        try
        {
            await Task.Delay(duration * 1000, token);
            if (!token.IsCancellationRequested && ActiveQuizzes.ContainsKey(quizId))
            {
                await Clients.All.SendAsync("QuizEnded", quizId);
                ActiveQuizzes.Remove(quizId, out _);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
