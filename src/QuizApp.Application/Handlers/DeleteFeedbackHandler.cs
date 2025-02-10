using MediatR;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;

public class DeleteFeedbackHandler(IUnitOfWork feedbackRepository) : IRequestHandler<DeleteFeedbackRequest>
{
    private readonly IUnitOfWork _feedbackRepository = feedbackRepository;

    public async Task Handle(DeleteFeedbackRequest request, CancellationToken cancellationToken)
    {
        await _feedbackRepository.FeedbackRepository.DeleteAsync(request.FeedbackId);
    }
}