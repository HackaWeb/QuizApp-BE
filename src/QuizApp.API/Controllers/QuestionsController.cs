using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Models;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody] CreateQuestionRequest request)
    {
        var question = await _mediator.Send(request);
        return Ok(question);
    }

    [HttpGet("{questionId:guid}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<QuestionDto>> GetQuestion(Guid questionId)
    {
        var query = new GetQuestionRequest(questionId);
        var question = await _mediator.Send(query);
        return Ok(question);
    }

    [HttpPut("{questionId:guid}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<QuestionDto>> UpdateQuestion(Guid questionId, [FromBody] UpdateQuestionModel model)
    {
        var command = new UpdateQuestionRequest(questionId, model.Text, model.Type);
        var updated = await _mediator.Send(command);
        return Ok(updated);
    }

    [HttpDelete("{questionId:guid}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteQuestion(Guid questionId)
    {
        var cmd = new DeleteQuestionRequest(questionId);
        await _mediator.Send(cmd);
        return NoContent();
    }

    [HttpPost("{quizId:guid}/questions")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Result> OverwriteQuestions(Guid quizId, [FromBody] List<OverwriteQuestionsDto> questions)
    {
        var request = new OverwriteQuestionsRequest(quizId, questions);
        await _mediator.Send(request);

        return Result.Success();
    }
}
