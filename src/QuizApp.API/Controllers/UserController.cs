using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Requests;

namespace QuizApp.API.Controllers;

public class UserController(IMediator mediator) : ControllerBase
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("user")]
    [Consumes("multipart/form-data")]
    public async Task<Result> UpdateUserProfile([FromForm] UpdateUserProfileRequest request)
    {
        var updatedResult = await mediator.Send(request);
        return updatedResult;
    }
}
