using Microsoft.AspNetCore.SignalR;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Collections.Concurrent;

namespace QuizApp.API.Hubs
{
    public class QuizTimerHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        private static readonly ConcurrentDictionary<string, QuizState> ActiveQuizzes
            = new ConcurrentDictionary<string, QuizState>();

        public QuizTimerHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task StartQuiz(string quizId, Guid userId)
        {
            if (!Guid.TryParse(quizId, out var quizGuid))
            {
                await Clients.Caller.SendAsync("InvalidQuizId", quizId);
                return;
            }

            var quiz = await _unitOfWork.QuizRepository.GetByIdAsync(quizGuid);
            if (quiz == null)
            {
                await Clients.Caller.SendAsync("QuizNotFound", quizId);
                return;
            }

            if (ActiveQuizzes.ContainsKey(quizId))
            {
                await Clients.Caller.SendAsync("QuizAlreadyStarted", quizId);
                return;
            }

            int duration = (int)quiz.Duration;
            var startTime = DateTime.UtcNow;
            var cancellationTokenSource = new CancellationTokenSource();

            var quizState = new QuizState
            {
                QuizId = quizGuid,
                UserId = userId,
                StartTime = startTime,
                Duration = duration,
                CancellationSource = cancellationTokenSource
            };

            ActiveQuizzes[quizId] = quizState;

            _ = StopQuizAfterDelay(quizId, userId, duration);

            await Clients.All.SendAsync("QuizStarted", quizId, userId, startTime, duration);
        }

        public async Task GetQuizTime(string quizId)
        {
            if (!ActiveQuizzes.TryGetValue(quizId, out var quizState))
            {
                await Clients.Caller.SendAsync("QuizNotActive", quizId);
                return;
            }

            var elapsed = (int)(DateTime.UtcNow - quizState.StartTime).TotalMinutes;
            var remainingTime = Math.Max(quizState.Duration - elapsed, 0);


            await Clients.Caller.SendAsync("UpdateQuizTime", quizId, remainingTime);
        }

        public async Task SubmitAnswers(string quizId, Guid userId, Dictionary<Guid, string> userAnswers)
        {
            if (!ActiveQuizzes.TryGetValue(quizId, out var quizState))
            {
                await Clients.Caller.SendAsync("QuizNotActive", quizId);
                return;
            }

            if (quizState.UserId != userId)
            {
                await Clients.Caller.SendAsync("UnauthorizedUser", quizId, userId);
                return;
            }

            var elapsed = (int)(DateTime.UtcNow - quizState.StartTime).TotalMinutes;
            if (elapsed >= quizState.Duration)
            {
                await Clients.Caller.SendAsync("QuizTimeExpired", quizId);
                return;
            }

            if (!Guid.TryParse(quizId, out var quizGuid))
            {
                await Clients.Caller.SendAsync("InvalidQuizId", quizId);
                return;
            }

            var quiz = await _unitOfWork.QuizRepository.GetByIdAsync(quizGuid);
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
                    bool isCorrect = question.ChoiceOptions
                        .Where(o => o.IsCorrect)
                        .Any(o => o.Title.Equals(userAnswer, StringComparison.OrdinalIgnoreCase));

                    if (isCorrect)
                        correctAnswers++;
                }
            }

            double score = totalQuestions == 0
                ? 0
                : (correctAnswers / (double)totalQuestions) * 100;

            var quizHistory = new QuizHistory
            {
                Id = Guid.NewGuid(),
                QuizId = quizGuid,
                UserId = userId,
                StartedAt = quizState.StartTime,
                FinishedAt = DateTime.UtcNow,
                Score = score
            };

            await _unitOfWork.QuizHistoryRepository.AddAsync(quizHistory);
            await _unitOfWork.SaveEntitiesAsync();

            await Clients.Caller.SendAsync("QuizCompleted", quizId, correctAnswers, totalQuestions, score);
            await Clients.All.SendAsync("UserFinishedQuiz", quizId, userId);

            ActiveQuizzes.Remove(quizId, out _);
            quizState.CancellationSource.Cancel();
        }

        private async Task StopQuizAfterDelay(string quizId, Guid userId, int duration)
        {
            try
            {
                await Task.Delay(duration * 60 * 1000);

                if (ActiveQuizzes.ContainsKey(quizId))
                {
                    ActiveQuizzes.Remove(quizId, out _);
                    await Clients.All.SendAsync("QuizEnded", quizId, userId);
                }
                else
                {
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

    public class QuizState
    {
        public Guid QuizId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public CancellationTokenSource CancellationSource { get; set; }
    }
}
