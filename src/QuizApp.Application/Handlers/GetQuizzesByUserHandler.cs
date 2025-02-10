using MediatR;
using QuizApp.Contracts.Rest.Models.UserProfile;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Specifications;

namespace QuizApp.Application.Handlers;

public class GetQuizzesByUserHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetQuizzesByUserRequest, GetQuizzesByUserResponse>
{
    public async Task<GetQuizzesByUserResponse> Handle(GetQuizzesByUserRequest request, CancellationToken cancellationToken)
    {
        var quizzes = await unitOfWork.QuizRepository.GetAllAsync();
        var quizHistory = await unitOfWork.QuizHistoryRepository.GetBySpecification(new QuizHistorySpecification(Guid.Parse(request.UserId), true));

        var pagedItems = quizzes
            .Where(x => x.OwnerId == Guid.Parse(request.UserId))
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(q => new Quiz(
                q.Id,
                q.Title,
                q.ImageUrl,
                q.Duration,
                quizHistory.Count(x => x.Quiz.Id == q.Id),
                q.Rate,
                (ushort)q.Questions.Count))
            .ToList();

        var response = new GetQuizzesByUserResponse
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalItems = quizzes.Count(),
            Items = pagedItems
        };

        return response;
    }
}
