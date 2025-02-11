using MediatR;
using QuizApp.Contracts.Rest.Models.UserProfile;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;

public class GetQuizzesByUserHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetQuizzesByUserIdCommand, GetQuizzesByUserResponse>
{
    public async Task<GetQuizzesByUserResponse> Handle(GetQuizzesByUserIdCommand request, CancellationToken cancellationToken)
    {
        var quizzes = await unitOfWork.QuizRepository.GetAllAsync();

        var pagedItems = quizzes
            .Where(x => x.OwnerId == request.UserId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(q => new Quiz(
                q.Id,
                q.Title,
                q.ImageUrl,
                q.Duration,
                quizzes.Count(x => x.Id == q.Id),
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
