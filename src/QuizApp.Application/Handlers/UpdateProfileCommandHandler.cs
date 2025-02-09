using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure.Repositories;
using System.Net;

namespace QuizApp.Application.Handlers;

public class UpdateProfileCommandHandler(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    IBlobStorageRepository blobRepository) : IRequestHandler<UpdateUserProfileRequest, UpdateUserProfileResponse>
{
    public async Task<UpdateUserProfileResponse> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext!.User;
        var currentUserId = userManager.GetUserId(currentUser);
        var isAdmin = currentUser.IsInRole("Admin");

        if (currentUserId != request.userId && !isAdmin)
        {
            throw new DomainException("You do not have permission to edit this profile.", (int)HttpStatusCode.Unauthorized);
        }

        var user = await userManager.FindByIdAsync(request.userId);
        if (user == null)
        {
            throw new DomainException("User not found.", (int)HttpStatusCode.BadRequest);
        }

        if (!string.IsNullOrWhiteSpace(request.email))
        {
            user.Email = request.email;
            user.UserName = request.email;
        }

        if (!string.IsNullOrWhiteSpace(request.firstName))
        {
            user.FirstName = request.firstName;
        }

        if (!string.IsNullOrWhiteSpace(request.lastName))
        {
            user.LastName = request.lastName;
        }

        if (request.Avatar is not null)
        {
            if (request.Avatar.ContentType != "image/png")
            {
                throw new DomainException("Only PNG images are allowed.", (int)HttpStatusCode.BadRequest);
            }

            using var stream = request.Avatar.OpenReadStream();
            user.AvatarUrl = await blobRepository.UploadAsync(
                stream,
                $"{currentUserId}.png",
                request.Avatar.ContentType,
                "imgs"
            );
        }
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            throw new DomainException("User info update ended with an error", (int)HttpStatusCode.InternalServerError);
            
        }
        return new UpdateUserProfileResponse(
            user.Id.ToString(),
            user.Email,
            user.FirstName,
            user.LastName,
            user.AvatarUrl);
    }
}
