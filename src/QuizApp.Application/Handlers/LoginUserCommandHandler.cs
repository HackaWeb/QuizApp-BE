using MediatR;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;

public class LoginUserCommandHandler(
    UserManager<User> userManager, 
    SignInManager<User> signInManager,
    IJwtTokenService jwtTokenService) : IRequestHandler<LoginUserRequest, Result>
{
    public async Task<Result> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure(["Invalid email or password."]);
        }

        var authResult = await signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (!authResult.Succeeded)
        {
            return Result.Failure(["Invalid email or password."]);
        }

        var token = jwtTokenService.GenerateToken(user);
        //TODO: change response
        return Result.Success();
    }
}
