using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Models;
using System.Security.Claims;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("user-profile/{userId?}")]
    [Consumes("multipart/form-data")]
    public async Task<UpdateUserProfileResponse> UpdateUserProfile(string? userId, [FromForm] UpdateUserProfileRequest request)
    {
        userId ??= User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new UpdateUserProfileCommand
        {
            UserId = userId,
            FirstName = request.FirstName,
            Avatar = request.Avatar,
            Email = request.Email,
            LastName = request.LastName,
        };
        var updatedResult = await mediator.Send(command);
        return updatedResult;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("user-profile")]
    public async Task<GetUserProfileResponse> GetUserProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var getUserProfileRequest = new GetUserProfileRequest(Guid.Parse(userId));
        var userProfile = await mediator.Send(getUserProfileRequest);

        return userProfile;
    }

    [HttpGet("user-profile/{userId:guid}")]
    public async Task<GetUserProfileResponse> GetUserProfileById(Guid userId)
    {
        var getUserProfileRequest = new GetUserProfileRequest(userId);
        var userProfile = await mediator.Send(getUserProfileRequest);

        return userProfile;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("user-profile/{userId?}")]
    public async Task<Result> DeleteUser(string? userId)
    {
        userId ??= User.FindFirstValue(ClaimTypes.NameIdentifier);

        var deleteUserProfileRequest = new DeleteUserProfileRequest(userId!);
        await mediator.Send(deleteUserProfileRequest);

        return Result.Success();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("user-profile/{userId?}/image")]
    public async Task<Result> DeleteUserImage(string? userId)
    {
        userId ??= User.FindFirstValue(ClaimTypes.NameIdentifier);
        await mediator.Send(new DeleteUserImageCommand(userId));

        return Result.Success();
    }
}
