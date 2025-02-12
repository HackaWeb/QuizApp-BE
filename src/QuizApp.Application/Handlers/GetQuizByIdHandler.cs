using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Net;

namespace QuizApp.Application.Handlers;

public class GetQuizByIdHandler(
    IUnitOfWork unitOfWork,
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper) : IRequestHandler<GetQuizByIdRequest, GetQuizByIdResponse>
{
    public async Task<GetQuizByIdResponse> Handle(GetQuizByIdRequest request, CancellationToken cancellationToken)
    {
        var quiz = await unitOfWork.QuizRepository.GetByIdAsync(request.id);

        if (quiz is null)
        {
            throw new DomainException("Quiz not found.",(int)HttpStatusCode.BadRequest);
        }

        var quizDto = mapper.Map<QuizModelWithOwner>(quiz);

        var user = await userManager.FindByIdAsync(quiz.OwnerId.ToString());
        var isAdmin = await userManager.IsInRoleAsync(user, "Admin");

        var quizzes = await unitOfWork.QuizRepository.GetAllAsync();

        var userRate = quizzes
            .Where(x => x.OwnerId == user.Id)
            .Select(x => (double?)x.Rate)
            .Average() ?? 0;
        quizDto.Owner = new OwnerDto();

        quizDto.Owner.Avatar = user.AvatarUrl;
        quizDto.Owner.FirstName = user.FirstName;
        quizDto.Owner.LastName = user.LastName;
        quizDto.Owner.Id = user.Id.ToString();
        quizDto.Owner.Email = user.Email;
        quizDto.Owner.IsAdmin = isAdmin;
        quizDto.Owner.Rate = userRate;

        var response = new GetQuizByIdResponse(quizDto);

        quiz.PassCount = quiz.PassCount + 1;
        await unitOfWork.SaveEntitiesAsync();
        return response;
    }
}
