using MediatR;
using Microsoft.AspNetCore.Http;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Security.Claims;

namespace QuizApp.Application.Handlers;

public class CreateFeedbackHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork feedbackRepository) : IRequestHandler<CreateFeedbackRequest, CreateFeedbackResponse>
{
    private readonly IUnitOfWork _unitOfWork = feedbackRepository;

    public async Task<CreateFeedbackResponse> Handle(CreateFeedbackRequest request, CancellationToken cancellationToken)
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }

        var feedback = new Feedback
        {
            Id = Guid.NewGuid(),
            QuizId = request.QuizId,
            UserId = userId,
            Text = request.Text,
            Rate = request.Rate,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.FeedbackRepository.AddAsync(feedback);

        return new CreateFeedbackResponse(feedback.Id, feedback.QuizId, feedback.Text, feedback.Rate, feedback.CreatedAt);
    }
}
