using MediatR;

namespace QuizApp.Contracts.Rest.Requests;

public record CheckQuizAnswersRequest(Guid QuizId, List<UserAnswerModel> Answers) : IRequest<CheckQuizAnswersResponse>;

public record UserAnswerModel(Guid QuestionId, List<Guid> SelectedOptionIds);

public record CheckQuizAnswersResponse(
    int TotalQuestions,
    int CorrectAnswers,
    double Score,
    List<QuestionResult> Results
);

public record QuestionResult(Guid QuestionId, bool IsCorrect);

