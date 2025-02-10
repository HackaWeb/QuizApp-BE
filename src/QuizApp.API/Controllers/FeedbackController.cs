using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
public class FeedbackController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id:guid}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<GetFeedbackByIdResponse> GetFeedbackById(Guid id)
    {
        var response = await _mediator.Send(new GetFeedbackByIdRequest(id));
        return response;
    }

    [HttpGet("quiz/{quizId:guid}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<List<GetFeedbackByIdResponse>> GetFeedbacksForQuiz(Guid quizId)
    {
        var response = await _mediator.Send(new GetFeedbacksForQuizRequest(quizId));
        return response;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackRequest request)
    {
        var response = await _mediator.Send(request);
        return CreatedAtAction(nameof(GetFeedbackById), new { id = response.FeedbackId }, response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Result> UpdateFeedback(Guid id, [FromBody] UpdateFeedbackRequest request)
    {
        if (id != request.FeedbackId) throw new DomainException("ID mismatch");
        await _mediator.Send(request);
        return Result.Success();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Result> DeleteFeedback(Guid id)
    {
        await _mediator.Send(new DeleteFeedbackRequest(id));
        return Result.Success();
    }
}
