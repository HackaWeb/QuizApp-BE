﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Specifications;
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
        var currentUser = httpContextAccessor.HttpContext!.User;
        var currentUserId = userManager.GetUserId(currentUser);
        var isAdmin = currentUser.IsInRole("Admin");

        var quiz = await unitOfWork.QuizRepository.GetSingleBySpecification(new QuizSpecification(quizId: request.id, isReadOnly: true));

        if (Guid.Parse(currentUserId) != quiz.OwnerId && !isAdmin)
        {
            throw new DomainException("You do not have permission to view this quiz.", (int)HttpStatusCode.Unauthorized);
        }
        var quizDto = mapper.Map<QuizModel>(quiz);

        return new GetQuizByIdResponse(quizDto);
    }
}
