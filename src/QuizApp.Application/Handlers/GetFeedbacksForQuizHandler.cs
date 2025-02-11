using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;

public class GetFeedbacksForQuizHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper) : IRequestHandler<GetFeedbacksForQuizRequest, List<GetFeedbackByIdResponse>>
{
    public async Task<List<GetFeedbackByIdResponse>> Handle(GetFeedbacksForQuizRequest request, CancellationToken cancellationToken)
    {
        var feedbacks = await unitOfWork.FeedbackRepository.GetByQuizIdAsync(request.QuizId);
        var responses = new List<GetFeedbackByIdResponse>();

        foreach (var item in feedbacks)
        {
            var user = await userManager.FindByIdAsync(item.UserId.ToString());
            var mappedUser = mapper.Map<UserDto>(user);

            responses.Add(new GetFeedbackByIdResponse(item.Id, item.QuizId, item.Text, item.Rate, item.CreatedAt, mappedUser));
        }

        return responses;
    }
}
