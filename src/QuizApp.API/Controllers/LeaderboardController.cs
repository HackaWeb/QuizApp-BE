using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Globalization;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController(IUnitOfWork unitOfWork, UserManager<User> userManager) : ControllerBase
{
    [HttpGet("{quizId:guid}")]
    public async Task<List<LeaderboardUserDto>> GetLeaderboard(Guid quizId)
    {
        var histories = await unitOfWork.QuizHistoryRepository.GetByQuizIdAsync(quizId);
        var result = new List<LeaderboardUserDto>();

        foreach (var history in histories)
        {

            var user = await userManager.FindByIdAsync(history.UserId.ToString());
            if (user == null)
            {
                continue;
            }


            var accuracy = history.Score;
            var timeSpent = (history.FinishedAt - history.StartedAt).TotalSeconds;
            var dateCompleted = history.FinishedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            var userDto = new OwnerDto
            {
                Id = user.Id.ToString(),
                Email = user.Email ?? "",
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? ""
            };

            var leaderboardUser = new LeaderboardUserDto
            {
                User = userDto,
                Accuracy = accuracy,
                TimeSpent = timeSpent,
                DateCompleted = dateCompleted
            };

            result.Add(leaderboardUser);
        }

        return result;
    }
}
