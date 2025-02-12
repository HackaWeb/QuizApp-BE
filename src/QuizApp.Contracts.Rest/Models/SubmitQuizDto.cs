using MediatR;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Models;

public class CheckQuizAnswersDto 
{
    public List<CheckQuestionAnswerDto> UserAnswers { get; set; } = new();
}

public class CheckQuestionAnswerDto
{
    public string QuestionId { get; set; }
    public int QuestionType { get; set; }
    public List<CheckAnswerOptionDto> Answers { get; set; } = new();
}

public class CheckAnswerOptionDto
{
    public Guid? OptionId { get; set; }
    public string? Text { get; set; }
}


public record SubmitQuiz(Guid quizId, CheckQuizAnswersDto anwers) : IRequest<CheckQuizAnswersResponse>;