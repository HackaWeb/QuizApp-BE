using AutoMapper;
using MediatR;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Models.UserProfile;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Exceptions;
using QuizApp.Infrastructure;
using System.Net;

namespace QuizApp.Application.Handlers;

public class GetQuizzesByUserHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetQuizzesByUserIdCommand, GetQuizzesByUserResponse>
{
    public async Task<GetQuizzesByUserResponse> Handle(GetQuizzesByUserIdCommand request, CancellationToken cancellationToken)
    {
        var quizzes = await unitOfWork.QuizRepository.GetAllAsync();

        var pagedItems = quizzes
            .Where(x => x.OwnerId == request.UserId)
            .Select(q => new Quiz(
                q.Id,
                q.Title,
                q.ImageUrl,
                q.Duration,
                quizzes.Count(x => x.Id == q.Id),
                q.Rate,
                (ushort)q.Questions.Count))
            .ToList();

        var response = new GetQuizzesByUserResponse
        {
            Quizzes = pagedItems
        };

        return response;
    }
}


public class GetQuizWithouQuestionsByUserHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetQuizWithoutQuestionsCommand, QuizModel>
{
    public async Task<QuizModel> Handle(GetQuizWithoutQuestionsCommand request, CancellationToken cancellationToken)
    {
        var quizzes = await unitOfWork.QuizRepository.GetAllAsync();

        var quiz = quizzes.FirstOrDefault(x => x.Id == request.quizId);
        if (quiz is null)
        {
            throw new DomainException("Quiz not found", (int)HttpStatusCode.NotFound);
        }
        quiz.Questions.Clear();

        return mapper.Map<QuizModel>(quiz);
    }
}
