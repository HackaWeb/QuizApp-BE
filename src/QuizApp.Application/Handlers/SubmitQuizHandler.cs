using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Net;

namespace QuizApp.Application.Handlers;
public class SubmitQuizHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) : IRequestHandler<SubmitQuiz, CheckQuizAnswersResponse>
{
    public async Task<CheckQuizAnswersResponse> Handle(SubmitQuiz request, CancellationToken cancellationToken)
    {
        var quiz = await unitOfWork.QuizRepository.GetByIdAsync(request.quizId);
        if (quiz == null)
        {
            throw new DomainException("Quiz not found.", (int)HttpStatusCode.NotFound);
        }

        int correctAnswers = 0;
        int totalQuestions = quiz.Questions.Count;

        foreach (var userQuestion in request.anwers.UserAnswers)
        {
            var matchingQuestion = quiz.Questions.FirstOrDefault(q => q.Id.ToString() == userQuestion.QuestionId);
            if (matchingQuestion == null) continue;

            bool isCorrect = userQuestion.Answers.All(userAnswer =>
            {
                if (userAnswer.OptionId.HasValue)
                {
                    if (userQuestion.QuestionType == (int)QuestionType.MultiSelect)
                    {
                        var correctOptionIds = matchingQuestion.ChoiceOptions
                            .Where(qOption => qOption.IsCorrect)
                            .Select(qOption => qOption.Id)
                            .ToHashSet();

                        var userOptionIds = userQuestion.Answers
                            .Where(a => a.OptionId.HasValue)
                            .Select(a => a.OptionId!.Value)
                            .ToHashSet();

                        return correctOptionIds.SetEquals(userOptionIds);
                    }
                    return matchingQuestion.ChoiceOptions.Any(qOption =>
                        qOption.Id == userAnswer.OptionId.Value && qOption.IsCorrect);
                }
                else if (!string.IsNullOrWhiteSpace(userAnswer.Text))
                {
                    return string.Equals(matchingQuestion.Text, userAnswer.Text, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            });

            if (isCorrect)
            {
                correctAnswers++;
            }
        }

        double score = totalQuestions == 0 ? 0 : ((double)correctAnswers / totalQuestions) * 100;

        var currentUser = httpContextAccessor.HttpContext!.User;
        var currentUserId = userManager.GetUserId(currentUser);

        await unitOfWork.QuizHistoryRepository.AddAsync(new Domain.Models.QuizHistory
        {
            QuizId = request.quizId,
            FinishedAt = DateTime.Now,
            Score = score,
            StartedAt = DateTime.Now.AddMinutes(-1 * quiz.Duration),
            UserId = Guid.Parse(currentUserId)
        });

        await unitOfWork.SaveEntitiesAsync();

        return new CheckQuizAnswersResponse
        {
            QuizId = request.quizId,
            CorrectAnswers = correctAnswers,
            TotalQuestions = totalQuestions,
            Score = score
        };
    }
}
