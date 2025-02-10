using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using System.Security.Claims;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("user-profile/update")]
    [Consumes("multipart/form-data")]
    public async Task<UpdateUserProfileResponse> UpdateUserProfile([FromForm] UpdateUserProfileRequest request)
    {
        request.UserId ??= User.FindFirstValue(ClaimTypes.NameIdentifier);

        var updatedResult = await mediator.Send(request);
        return updatedResult;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("user-profile")]
    public async Task<GetUserProfileResponse> GetUserProfile([FromQuery] string? userId)
    {
        userId ??= User.FindFirstValue(ClaimTypes.NameIdentifier);

        var getUserProfileRequest = new GetUserProfileRequest(userId);
        var userProfile = await mediator.Send(getUserProfileRequest);

        return userProfile;
    }
}
