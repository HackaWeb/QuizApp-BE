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

    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateQuiz([FromForm]CreateQuizRequest request)
    {
        await mediator.Send(request);
    }

    [HttpGet("id")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<GetQuizByIdResponse> GetQuizById([FromQuery] string id)
    {
        var request = new GetQuizByIdRequest(Guid.Parse(id));
        var quiz = await mediator.Send(request);

        return quiz;
    }
}
