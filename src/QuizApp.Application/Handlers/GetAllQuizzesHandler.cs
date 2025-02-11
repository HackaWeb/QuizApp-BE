using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Linq;

namespace QuizApp.Application.Handlers;

public class GetAllQuizzesHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UserManager<User> userManager) : IRequestHandler<GetAllQuizzesRequest, GetAllQuizzesResponse>
{
    public async Task<GetAllQuizzesResponse> Handle(GetAllQuizzesRequest request, CancellationToken cancellationToken)
    {
        var domainQuizzes = await unitOfWork.QuizRepository.GetAllAsync();

        var ownerIds = domainQuizzes.Select(q => q.OwnerId.ToString()).Distinct();
        var users = (await userManager.Users.Where(u => ownerIds.Contains(u.Id.ToString())).ToListAsync())
            .ToDictionary(u => u.Id);

        var userRatings = domainQuizzes
            .GroupBy(q => q.OwnerId)
            .ToDictionary(g => g.Key, g => g.Average(q => (double?)q.Rate) ?? 0);

        var quizzes = domainQuizzes
            .Where(q => users.ContainsKey(q.OwnerId))
            .Where(q => string.IsNullOrEmpty(request.titleFilter) || request.titleFilter.Length < 3 ||
                        q.Title.Contains(request.titleFilter, StringComparison.OrdinalIgnoreCase)) 
            .Select(item =>
            {
                var user = users[item.OwnerId];
                var isAdmin = userManager.IsInRoleAsync(user, "Admin").Result;
                var userRate = userRatings.ContainsKey(item.OwnerId) ? userRatings[item.OwnerId] : 0;

                return new QuizModelWithOwner
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    NumberOfPasses = item.PassCount,
                    Rate = item.Rate,
                    Owner = new OwnerDto
                    {
                        Avatar = user.AvatarUrl,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Id = user.Id.ToString(),
                        Email = user.Email,
                        IsAdmin = isAdmin,
                        Rate = userRate
                    }
                };
            })
            .ToList();

        quizzes = SortQuizzes(quizzes, request.sortType);

        return new GetAllQuizzesResponse(quizzes);
    }
    private List<QuizModelWithOwner> SortQuizzes(List<QuizModelWithOwner> quizzes, SortType sortType)
    {
        return sortType switch
        {
            SortType.Rating => quizzes.OrderByDescending(q => q.Rate).ToList(),
            SortType.NumberOfPasses => quizzes.OrderByDescending(q => q.NumberOfPasses).ToList(),
            SortType.Alphabet => quizzes.OrderBy(q => q.Title).ToList(),
            SortType.AuthorRating => quizzes.OrderByDescending(q => q.Owner.Rate).ToList(),
            _ => quizzes
        };
    }

}
