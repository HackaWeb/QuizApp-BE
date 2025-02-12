﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Net;

namespace QuizApp.Application.Handlers;

public class DeleteUserProfileHandler(
    UserManager<User> userManager, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserProfileRequest>
{
    public async Task Handle(DeleteUserProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new DomainException("User not found.");
        }

        var deleteResult = await userManager.DeleteAsync(user);

        if (!deleteResult.Succeeded)
        {
            throw new DomainException("Failed to delete user.", (int)HttpStatusCode.BadRequest, deleteResult.Errors.ToDictionary(x => x.Code, x => x.Description));
        }

        var quizzes = await unitOfWork.QuizRepository.GetAllAsync();
        var userQuizes = quizzes.Where(x => x.OwnerId == user.Id).ToList();


        foreach (var item in userQuizes)
        {
            await unitOfWork.QuizRepository.DeleteAsync(item.Id);
        }

        await unitOfWork.SaveEntitiesAsync();
    }
}
