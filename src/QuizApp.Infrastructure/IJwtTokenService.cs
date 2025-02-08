using QuizApp.Domain.Models;

namespace QuizApp.Infrastructure;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
