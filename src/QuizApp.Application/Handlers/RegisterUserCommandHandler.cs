using MediatR;
using Microsoft.AspNetCore.Identity;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Net;

namespace QuizApp.Application.Handlers;

public class RegisterUserCommandHandler(
    UserManager<User> userManager, 
    IJwtTokenService jwtTokenService) : IRequestHandler<RegisterUserRequest, TokenResponse>
{
    public async Task<TokenResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
        };

        var identityResult = await userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            throw new DomainException("Error during registration.", (int)HttpStatusCode.BadRequest, identityResult.Errors.ToDictionary(x => x.Code, x => x.Description));
        }

        await userManager.AddToRoleAsync(user, "User");

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtTokenService.GenerateToken(user, roles);
        return new TokenResponse(token);
    }
}
