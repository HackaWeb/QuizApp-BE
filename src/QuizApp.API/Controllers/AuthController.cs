using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Requests;

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

    public async Task<IActionResult> Login([FromBody]LoginUserRequest request)
    {
        var authResult = await mediator.Send(request);
        if (!authResult.IsSuccess)
        {
            return Unauthorized(authResult.Errors);
        }

        return Ok("Login successful.");
    }
}
