using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Net;

namespace QuizApp.Application.Handlers;


public class DeleteUserImageHandler(
    IUnitOfWork unitOfWork,
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<DeleteUserImageCommand>
{
    public async Task Handle(DeleteUserImageCommand request, CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext!.User;
        var currentUserId = userManager.GetUserId(currentUser);
        var isAdmin = currentUser.IsInRole("Admin");

        if (currentUserId != request.UserId && !isAdmin)
        {
            throw new DomainException("You do not have permission to edit this profile.", (int)HttpStatusCode.Unauthorized);
        }

        var user = await userManager.FindByIdAsync(request.UserId!);
        user.AvatarUrl = null;

        var updateResult = await userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            throw new DomainException("User info update ended with an error", (int)HttpStatusCode.InternalServerError);
        }

        await unitOfWork.SaveEntitiesAsync();
    }
}
