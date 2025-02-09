using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Requests;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost("update-profile")]
    [Consumes("multipart/form-data")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Result> UpdateUserProfile([FromForm] UpdateUserProfileRequest request)
    {
        var updatedResult = await mediator.Send(request);
        return updatedResult;
    }
}
