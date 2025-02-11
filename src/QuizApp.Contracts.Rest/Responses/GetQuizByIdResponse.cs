using QuizApp.Contracts.Rest.Models.Quiz;

namespace QuizApp.Contracts.Rest.Responses;

public record GetQuizByIdResponse(QuizModelWithOwner quiz)
{
}
