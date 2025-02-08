using Microsoft.AspNetCore.Identity;

namespace QuizApp.Domain.Models;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; }
}
