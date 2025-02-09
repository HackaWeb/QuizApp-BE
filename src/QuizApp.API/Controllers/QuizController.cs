using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
public class QuizController(IMediator mediator) : ControllerBase
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("{userId}")]
    public async Task<GetQuizzesByUserResponse> GetQuizzesByUser([FromBody]GetQuizzesByUserRequest request)
    {
        var quizzes = await mediator.Send(request);
        return quizzes;
    }
}
