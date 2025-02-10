using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Models.Quiz;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<QuizModel> CreateQuiz([FromBody]CreateQuizRequest request)
    {
        var response = await mediator.Send(request);
        return response;
    }

    [HttpPost("media/upload")]
    [Consumes("multipart/form-data")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Result> CreateQuizMedia(string quizId, IFormFile file)
    {
        await mediator.Send(new UploadQuizFileRequest
        {
            File = file,
            QuizId = quizId
        });

        return Result.Success();
    }

    [HttpPost("media/question/upload")]
    [Consumes("multipart/form-data")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Result> CreateQuestionMedia(string questionId, IFormFile file)
    {
        await mediator.Send(new UploadQuestionMediaRequest
        {
            File = file,
            QuestionId = questionId
        });

        return Result.Success();
    }

    [HttpGet("id")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<GetQuizByIdResponse> GetQuizById([FromQuery] string id)
    {
        var request = new GetQuizByIdRequest(Guid.Parse(id));
        var quiz = await mediator.Send(request);

        return quiz;
    }

    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Result> DeleteQuiz([FromQuery] string quizId)
    {
        await mediator.Send(new DeleteQuizRequest(Guid.Parse(quizId)));
        return Result.Success();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<GetAllQuizzesResponse> GetAllQuizzes()
    {
        var quizzes = await mediator.Send(new GetAllQuizzesRequest());
        return quizzes;
    }
}
