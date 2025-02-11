using MediatR;
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
    public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody] CreateQuestionRequest request)
    {
        var question = await _mediator.Send(request);
        return Ok(question);
    }

    [HttpGet("{questionId:guid}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion(Guid questionId)
    {
        var query = new GetQuestionRequest(questionId);
        var question = await _mediator.Send(query);
        return Ok(question);
    }

    [HttpPut("{questionId:guid}")]
    public async Task<ActionResult<QuestionDto>> UpdateQuestion(Guid questionId, [FromBody] UpdateQuestionModel model)
    {
        var command = new UpdateQuestionRequest(questionId, model.Text, model.Type);
        var updated = await _mediator.Send(command);
        return Ok(updated);
    }

    [HttpDelete("{questionId:guid}")]
    public async Task<IActionResult> DeleteQuestion(Guid questionId)
    {
        var cmd = new DeleteQuestionRequest(questionId);
        await _mediator.Send(cmd);
        return NoContent();
    }
}
