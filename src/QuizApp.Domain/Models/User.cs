using Microsoft.AspNetCore.Identity;

namespace QuizApp.Domain.Models;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; }
}
