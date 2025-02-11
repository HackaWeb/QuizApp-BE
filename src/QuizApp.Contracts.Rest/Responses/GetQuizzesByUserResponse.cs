using QuizApp.Contracts.Rest.Models.UserProfile;

namespace QuizApp.Contracts.Rest.Responses;

public class GetQuizzesByUserResponse
{
    public List<Quiz> Quizzes { get; set; }
}
