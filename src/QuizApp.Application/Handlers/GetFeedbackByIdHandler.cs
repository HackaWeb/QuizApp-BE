using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.Application.Handlers;
public class GetFeedbackByIdHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper) : IRequestHandler<GetFeedbackByIdRequest, GetFeedbackByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetFeedbackByIdResponse> Handle(GetFeedbackByIdRequest request, CancellationToken cancellationToken)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(request.FeedbackId);

        if (feedback is null)
            throw new KeyNotFoundException($"Feedback with ID {request.FeedbackId} not found.");

        var user = await userManager.FindByIdAsync(feedback.UserId.ToString());
        var mappedUser = mapper.Map<UserDto>(user);

        return new GetFeedbackByIdResponse(feedback.Id, feedback.QuizId, feedback.Text, feedback.Rate, feedback.CreatedAt, mappedUser);
    }
}
