using MediatR;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Models;

namespace QuizApp.Application.Handlers;

public class RegisterUserCommandHandler(UserManager<User> userManager) : IRequestHandler<RegisterUserRequest, Result>
{
    public async Task<Result> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
        };

        var identityResult = await userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            return Result.Failure(identityResult.Errors.Select(e => e.Description).ToList());
        }

        return Result.Success();
    }
}
