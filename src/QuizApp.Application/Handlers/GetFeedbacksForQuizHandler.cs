using MediatR;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;

public class GetFeedbacksForQuizHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetFeedbacksForQuizRequest, List<GetFeedbackByIdResponse>>
{
    public async Task<List<GetFeedbackByIdResponse>> Handle(GetFeedbacksForQuizRequest request, CancellationToken cancellationToken)
    {
        var feedbacks = await unitOfWork.FeedbackRepository.GetByQuizIdAsync(request.QuizId);

        return feedbacks.Select(f => new GetFeedbackByIdResponse(f.Id, f.QuizId, f.Text, f.Rate, f.CreatedAt)).ToList();
    }
}
