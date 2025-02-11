using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Exceptions;
using System.Net;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
public class QuizController(IMediator mediator) : ControllerBase
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("/user/{userId}")]
    public async Task<GetQuizzesByUserResponse> GetQuizzesByUser([FromRoute] string userId)
    {
        if (!Guid.TryParse(userId, out var userGuid))
        {
            throw new DomainException("Invalid user ID format.", (int)HttpStatusCode.BadRequest);
        }

        var command = new GetQuizzesByUserIdCommand(userGuid);
        var quizzes = await mediator.Send(command);
        return quizzes;
    }

    [HttpGet("completed/{userId}")]
    public async Task<List<QuizModel>> GetCompleteQuizzesByUser([FromRoute] string userId)
    {

        if (!Guid.TryParse(userId, out var userGuid))
        {
            throw new DomainException("Invalid user ID format.", (int)HttpStatusCode.BadRequest);
        }

        var command = new GetUserCompletedQuests(userGuid.ToString());
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
        if (!Guid.TryParse(id, out var quizId))
        {
            throw new DomainException("Quiz not found.", (int)HttpStatusCode.NotFound);
        }

        var request = new GetQuizByIdRequest(quizId);
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
    public async Task<QuizModel> GetQuizByIdWithoutQuestions(string quizId)
    {
        var command = new GetQuizWithoutQuestionsCommand(Guid.Parse(quizId));
        var quiz = await mediator.Send(command);

        return quiz;
    }

    [HttpGet("/questions/{quizId}")]
    public async Task<List<QuestionWithOptions>> GetQuizQuestions([FromRoute]string quizId)
    {
        if(!Guid.TryParse(quizId, out var parsedQuizId))
        {
            throw new DomainException("Invalid quiz ID format.", (int)HttpStatusCode.BadRequest);
        }
        var questions = await mediator.Send(new GetQuizQuestions(parsedQuizId));
        return questions;
    }
}
