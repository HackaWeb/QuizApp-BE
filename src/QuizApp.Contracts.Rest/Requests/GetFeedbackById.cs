using MediatR;

namespace QuizApp.Contracts.Rest.Requests;
public record GetFeedbackByIdRequest(Guid FeedbackId) : IRequest<GetFeedbackByIdResponse>;

public record GetFeedbackByIdResponse(Guid FeedbackId, Guid QuizId, string Text, ushort Rate, DateTime CreatedAt, UserDto user);

public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Avatar { get; set; }
}