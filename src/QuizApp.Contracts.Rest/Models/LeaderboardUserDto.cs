using QuizApp.Contracts.Rest.Models.Quiz;

namespace QuizApp.Contracts.Rest.Models;
public class LeaderboardUserDto
{
    public OwnerDto User { get; set; } = null!;
    public double Accuracy { get; set; }
    public double TimeSpent { get; set; }
    public string DateCompleted { get; set; } = string.Empty;
}

