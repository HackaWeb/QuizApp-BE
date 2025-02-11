using MediatR;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.Contracts.Rest.Requests;

public class GetQuizzesByUserRequest 
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public record GetQuizzesByUserIdCommand(Guid UserId, int PageSize, int PageNumber) : IRequest<GetQuizzesByUserResponse>;

public record GetQuizWithoutQuestionsCommand(Guid quizId) : IRequest<QuizModel>;
