using MediatR;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;
public class UpdateFeedbackHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateFeedbackRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateFeedbackRequest request, CancellationToken cancellationToken)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(request.FeedbackId);

        if (feedback is null)
            throw new KeyNotFoundException($"Feedback with ID {request.FeedbackId} not found.");

        feedback.Text = request.Text;
        feedback.Rate = request.Rate;

        await _unitOfWork.FeedbackRepository.UpdateAsync(feedback);
    }
}
