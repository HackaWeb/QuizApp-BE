namespace QuizApp.Contracts.Rest.Responses;

public class CheckQuizAnswersResponse
{
    public Guid QuizId { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public double Score { get; set; }
}
