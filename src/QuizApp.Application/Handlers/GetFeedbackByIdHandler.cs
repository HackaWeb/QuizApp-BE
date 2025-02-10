using MediatR;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.Application.Handlers;
public class GetFeedbackByIdHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetFeedbackByIdRequest, GetFeedbackByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetFeedbackByIdResponse> Handle(GetFeedbackByIdRequest request, CancellationToken cancellationToken)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(request.FeedbackId);

        if (feedback is null)
            throw new KeyNotFoundException($"Feedback with ID {request.FeedbackId} not found.");

        return new GetFeedbackByIdResponse(feedback.Id, feedback.QuizId, feedback.Text, feedback.Rate, feedback.CreatedAt);
    }
}
