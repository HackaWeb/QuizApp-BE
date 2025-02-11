using Microsoft.AspNetCore.SignalR;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;
using System.Collections.Concurrent;

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