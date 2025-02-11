using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;

public class GetAllQuizzesHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UserManager<User> userManager) : IRequestHandler<GetAllQuizzesRequest, GetAllQuizzesResponse>
{
    public async Task<GetAllQuizzesResponse> Handle(GetAllQuizzesRequest request, CancellationToken cancellationToken)
    {
        var quizzes = new List<QuizModelWithOwner>();
        var domainQuizzes = await unitOfWork.QuizRepository.GetAllAsync();

        foreach (var item in domainQuizzes)
        {
            var user = await userManager.FindByIdAsync(item.OwnerId.ToString());
            if (user is null)
            {
                continue;
            }
            var quizDto = mapper.Map<QuizModelWithOwner>(item);
            var isAdmin = await userManager.IsInRoleAsync(user, "Admin");

            quizDto.Owner = new OwnerDto();

            var userRate = domainQuizzes
                .Where(x => x.OwnerId == user.Id)
                .Select(x => (double?)x.Rate)
                .Average() ?? 0;
                quizDto.Owner = new OwnerDto();

            quizDto.Owner.Avatar = user.AvatarUrl;
            quizDto.Owner.FirstName = user.FirstName;
            quizDto.Owner.LastName = user.LastName;
            quizDto.Owner.Id = user.Id.ToString();
            quizDto.Owner.Email = user.Email;
            quizDto.Owner.IsAdmin = isAdmin;
            quizDto.Owner.Rate = userRate;

            quizzes.Add(quizDto);
        }
        

        return new GetAllQuizzesResponse(quizzes);
    }
}
