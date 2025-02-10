using MediatR;
using QuizApp.Contracts.Rest.Models.Quiz;

namespace QuizApp.Contracts.Rest.Requests;

public record CreateQuizRequest(CreateQuizModel Quiz) : IRequest<QuizModel>;
