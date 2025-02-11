using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Net;

namespace QuizApp.Application.Handlers;

public class OverwriteQuestionsHandler(
    IUnitOfWork unitOfWork, 
    IHttpContextAccessor httpContextAccessor,
    UserManager<User> userManager) : IRequestHandler<OverwriteQuestionsRequest>
{
    public async Task Handle(OverwriteQuestionsRequest request, CancellationToken cancellationToken)
    {
        var quiz = await unitOfWork.QuizRepository.GetByIdAsync(request.quizId);
        if (quiz == null)
        {
            throw new DomainException($"Quiz with ID {request.quizId} not found.", (int)HttpStatusCode.BadRequest);
        }

        var currentUser = httpContextAccessor.HttpContext!.User;
        var currentUserId = userManager.GetUserId(currentUser);
        var isAdmin = currentUser.IsInRole("Admin");

        if (Guid.Parse(currentUserId) != quiz.OwnerId && !isAdmin)
        {
            throw new DomainException("You do not have permission to edit this profile.", (int)HttpStatusCode.Unauthorized);
        }

        quiz.Questions.Clear();

        var questions = new List<Question>();

        foreach (var item in request.questions)
        {
            var question = new Question
            {
                Id = item.Id,
                QuizId = item.QuizId,
                Text = item.Text,
                Type = (Domain.QuestionType)item.Type,
                ChoiceOptions = new List<AnswerOption>()
            };


            if (item.Options is { Count: > 0 })
            {
                foreach (var optModel in item.Options)
                {
                    question.ChoiceOptions.Add(new AnswerOption
                    {
                        Id = Guid.NewGuid(),
                        Title = optModel.Title,
                        IsCorrect = optModel.IsCorrect
                    });
                }
            }
        }

        quiz.Questions = questions;
        await unitOfWork.SaveEntitiesAsync();
    }
}


public class GetQuizQuestionsHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor,
    UserManager<User> userManager) : IRequestHandler<GetQuizQuestions, List<QuestionWithOptions>>
{
    public async Task<List<QuestionWithOptions>> Handle(GetQuizQuestions request, CancellationToken cancellationToken)
    {
        var quiz = await unitOfWork.QuizRepository.GetByIdAsync(request.quizId);

        var questions = mapper.Map<List<QuestionWithOptions>>(quiz.Questions);

        return questions;
    }
}
