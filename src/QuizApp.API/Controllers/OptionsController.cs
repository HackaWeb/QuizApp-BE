using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Models;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OptionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<OptionDto>> CreateOption([FromBody] CreateOptionRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("{optionId:guid}")]
    public async Task<ActionResult<OptionDto>> GetOption(Guid optionId)
    {
        var query = new GetOptionRequest(optionId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{optionId:guid}")]
    public async Task<ActionResult<OptionDto>> UpdateOption(Guid optionId, [FromBody] UpdateOptionModel model)
    {
        var cmd = new UpdateOptionRequest(optionId, model.Title, model.IsCorrect);
        var updated = await _mediator.Send(cmd);
        return Ok(updated);
    }

    [HttpDelete("{optionId:guid}")]
    public async Task<IActionResult> DeleteOption(Guid optionId)
    {
        var cmd = new DeleteOptionRequest(optionId);
        await _mediator.Send(cmd);
        return NoContent();
    }
}
