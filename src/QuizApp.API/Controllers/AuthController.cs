using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.API.Controllers;

public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterUserRequest request)
    {
        var registrationResult = await mediator.Send(request);
        if (!registrationResult.IsSuccess)
        {
            return BadRequest(registrationResult.Errors);
        }

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<LoginUserResponse> Login([FromBody]LoginUserRequest request)
    {
        var authResult = await mediator.Send(request);
        return authResult;
    }
}
