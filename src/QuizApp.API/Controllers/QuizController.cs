using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using System.Security.Claims;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
public class QuizController(IMediator mediator) : ControllerBase
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("{userId?}")]
    public async Task<GetQuizzesByUserResponse> GetQuizzesByUser(string? userId, [FromBody] GetQuizzesByUserRequest request)
    {
        userId ??= User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new GetQuizzesByUserIdCommand(Guid.Parse(userId), request.PageSize, request.PageNumber);
        var quizzes = await mediator.Send(command);
        return quizzes;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("completed/{userId?}")]
    public async Task<List<QuizModel>> GetCompleteQuizzesByUser(string? userId)
    {
        userId ??= User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new GetUserCompletedQuests(userId);
        var quizzes = await mediator.Send(command);
        return quizzes;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<QuizModel> CreateQuiz([FromBody] CreateQuizRequest request)
    {
        var response = await mediator.Send(request);
        return response;
    }

    [HttpPost("media/upload/{quizId}")]
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

    [HttpPost("media/question/upload/{questionId}")]
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

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<GetQuizByIdResponse> GetQuizById(string id)
    {
        var request = new GetQuizByIdRequest(Guid.Parse(id));
        var quiz = await mediator.Send(request);

        return quiz;
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Result> DeleteQuiz(string id)
    {
        await mediator.Send(new DeleteQuizRequest(Guid.Parse(id)));
        return Result.Success();
    }

    [HttpGet("all")]
    public async Task<GetAllQuizzesResponse> GetAllQuizzes()
    {
        var quizzes = await mediator.Send(new GetAllQuizzesRequest());
        return quizzes;
    }

    [HttpGet("without-questions/{quizId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<QuizModel> GetQuizByIdWithouQuestions(string quizId)
    {
        var command = new GetQuizWithoutQuestionsCommand(Guid.Parse(quizId));
        var quiz = await mediator.Send(command);

        return quiz;
    }
}
