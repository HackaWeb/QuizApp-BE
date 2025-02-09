using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure.Repositories;
using System.Net;

namespace QuizApp.Application.Handlers;

public class UpdateProfileCommandHandler(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    IBlobStorageRepository blobRepository) : IRequestHandler<UpdateUserProfileRequest, Result>
{
    public async Task<Result> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext!.User;
        var currentUserId = userManager.GetUserId(currentUser);
        var isAdmin = currentUser.IsInRole("Admin");

        if (currentUserId != request.UserId && !isAdmin)
        {
            throw new DomainException("You do not have permission to edit this profile.", (int)HttpStatusCode.Unauthorized);
        }

        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new DomainException("User not found.", (int)HttpStatusCode.BadRequest);
        }

        user.Email = request.Email;
        user.UserName = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        if (request.Avatar is not null)
        {
            using var stream = request.Avatar.OpenReadStream();
            user.AvatarUrl = await blobRepository.UploadAsync(stream, $"{DateTime.Now}_{Guid.NewGuid()}", request.Avatar.ContentType, $"{currentUserId}");
        }

        var updateResult = await userManager.UpdateAsync(user);
        return updateResult.Succeeded
            ? Result.Success()
            : Result.Failure(["Failed to update profile."]);
    }
}
