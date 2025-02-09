using MediatR;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Net;

namespace QuizApp.Application.Handlers;

public class LoginUserCommandHandler(
    UserManager<User> userManager, 
    SignInManager<User> signInManager,
    IJwtTokenService jwtTokenService) : IRequestHandler<LoginUserRequest, TokenResponse>
{
    public async Task<TokenResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new DomainException("Invalid email or password.", (int)HttpStatusCode.Unauthorized);
        }

        var authResult = await signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (!authResult.Succeeded)
        {
            throw new DomainException("Invalid email or password.", (int)HttpStatusCode.Unauthorized);
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtTokenService.GenerateToken(user, roles);

        return new TokenResponse(token);
    }
}
