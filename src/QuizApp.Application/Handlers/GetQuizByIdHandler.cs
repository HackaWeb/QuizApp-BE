using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;

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
        var quizDto = mapper.Map<QuizModel>(quiz);

        return new GetQuizByIdResponse(quizDto);
    }
}
