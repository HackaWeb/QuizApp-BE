﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Linq;
using System.Net;

namespace QuizApp.Application.Handlers;

public class GetUserProfileHandler(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork) : IRequestHandler<GetUserProfileRequest, GetUserProfileResponse>
{
    public async Task<GetUserProfileResponse> Handle(GetUserProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId!.ToString());
        if (user is null)
        {
            throw new DomainException("User not found.", (int)HttpStatusCode.InternalServerError);
        }

        var isAdmin = await userManager.IsInRoleAsync(user, "Admin");

        var quizzes = await unitOfWork.QuizRepository.GetAllAsync();
        var quizCount = quizzes.Count(x => x.OwnerId == request.UserId);

        var userRate = quizzes
            .Where(x => x.OwnerId == request.UserId)
            .Select(x => (double?)x.Rate)
            .Average() ?? 0;

        var userProfile = new GetUserProfileResponse(
            user.FirstName,
            user.LastName,
            user.Email!,
            user.AvatarUrl,
            isAdmin,
            userRate,
            request.UserId.ToString());

        return userProfile;
    }
}
