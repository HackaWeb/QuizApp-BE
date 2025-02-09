using MediatR;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Specifications;
using System.Net;

namespace QuizApp.Application.Handlers;

public class GetUserProfileHandler(
    UserManager<User> userManager,
    IUnitOfWork unitOfWork) : IRequestHandler<GetUserProfileRequest, GetUserProfileResponse>
{
    public async Task<GetUserProfileResponse> Handle(GetUserProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            throw new DomainException("User not found.", (int)HttpStatusCode.InternalServerError);
        }

        var roles = await userManager.GetRolesAsync(user);
        var completedQuizzez = await unitOfWork.QuizHistoryRepository.GetBySpecification(new QuizHistorySpecification(user.Id, true));
        var createdQuizzez = await unitOfWork.QuizRepository.GetBySpecification(new QuizSpecification(user.Id, isReadOnly: true));

        var userProfile = new GetUserProfileResponse(
            user.FirstName,
            user.LastName,
            user.Email!,
            user.AvatarUrl,
            (uint)completedQuizzez.Count,
            (uint)createdQuizzez.Count,
            roles.Contains("Admin"));

        return userProfile;
    }
}
