namespace QuizApp.Contracts.Rest.Models;

public class Result
{
    public bool IsSuccess { get; private set; }
    public List<string>? Errors { get; private set; }
    public static Result Success() => new Result { IsSuccess = true };
    public static Result Failure(List<string> errors) => new Result { IsSuccess = false, Errors = errors };
}
