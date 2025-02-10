using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;
using System.Net;
using Xabe.FFmpeg;

namespace QuizApp.Application.Handlers;

public class CreateQuizHandler(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<CreateQuizHandler> logger,
    IUnitOfWork unitOfWork,
    IBlobStorageRepository blobStorageRepository,
    IMapper mapper) : IRequestHandler<CreateQuizRequest, QuizModel>
{
    public async Task<QuizModel> Handle(CreateQuizRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("{handler} started executing!", nameof(CreateQuizHandler));

        var currentUser = httpContextAccessor.HttpContext!.User;
        var currentUserId = userManager.GetUserId(currentUser);

        if (string.IsNullOrEmpty(currentUserId))
        {
            throw new DomainException("User not found.", (int)HttpStatusCode.Unauthorized);
        }

        var quiz = new Quiz
        {
            Title = request.Quiz.Title,
            Description = request.Quiz.Description,
            Duration = request.Quiz.Duration,
            OwnerId = Guid.Parse(currentUserId),
            Questions = new List<Question>()
        };

        foreach (var item in request.Quiz.Questions)
        {
            var question = new Question
            {
                Id = item.QuestionId,
                Text = item.Title,
                Type = (QuestionType)item.Type,
                ChoiceOptions = item.Options?.Select(o => new AnswerOption
                {
                    Title = o.Title,
                    IsCorrect = o.IsCorrect
                }).ToList()
            };

            quiz.Questions.Add(question);
        }

        await unitOfWork.QuizRepository.AddAsync(quiz);
        await unitOfWork.SaveEntitiesAsync();

        logger.LogInformation("Quiz '{QuizTitle}' created successfully!", quiz.Title);

        var createdQuiz = await unitOfWork.QuizRepository.GetByIdAsync(quiz.Id);
        return mapper.Map<QuizModel>(createdQuiz);
    }
}
