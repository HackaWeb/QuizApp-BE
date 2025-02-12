using MediatR;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;

public class CheckQuizAnswersHandler(IUnitOfWork unitOfWork) : IRequestHandler<CheckQuizAnswersRequest, CheckQuizAnswersResponseOutdated>
{
    public async Task<CheckQuizAnswersResponseOutdated> Handle(CheckQuizAnswersRequest request, CancellationToken cancellationToken)
    {
        var quiz = await unitOfWork.QuizRepository.GetByIdAsync(request.QuizId);

        if (quiz == null)
        {
            throw new KeyNotFoundException($"Quiz with ID {request.QuizId} not found.");
        }

        int totalQuestions = quiz.Questions.Count;
        int correctAnswers = 0;
        var results = new List<QuestionResult>();

        foreach (var userAnswer in request.Answers)
        {
            var question = quiz.Questions.FirstOrDefault(q => q.Id == userAnswer.QuestionId);
            if (question == null)
            {
                results.Add(new QuestionResult(userAnswer.QuestionId, false));
                continue;
            }

            var correctOptionIds = question.ChoiceOptions
                .Where(o => o.IsCorrect)
                .Select(o => o.Id)
                .ToHashSet();

            var userOptionIds = userAnswer.SelectedOptionIds.ToHashSet();

            bool isCorrect = userOptionIds.SetEquals(correctOptionIds);
            if (isCorrect) correctAnswers++;

            results.Add(new QuestionResult(userAnswer.QuestionId, isCorrect));
        }

        double score = (totalQuestions > 0) ? ((double)correctAnswers / totalQuestions) * 100 : 0;

        return new CheckQuizAnswersResponseOutdated(totalQuestions, correctAnswers, score, results);
    }
}
