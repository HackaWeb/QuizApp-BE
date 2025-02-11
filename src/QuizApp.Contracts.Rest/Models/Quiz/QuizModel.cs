namespace QuizApp.Contracts.Rest.Models.Quiz;

public class QuizModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public string ImageUrl { get; set; }

    public ushort? Rate { get; set; }

    public uint PassCount { get; set; }

    public Guid OwnerId { get; set; }

    public uint Duration { get; set; }

    public List<QuestionModel> Questions { get; set; }

    public List<FeedbackModel> Feedbacks { get; set; }
}

public class QuizModelWithOwner
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public string ImageUrl { get; set; }

    public ushort? Rate { get; set; }

    public uint NumberOfPasses { get; set; }

    public OwnerDto Owner { get; set; }

    public uint Duration { get; set; }

    public List<QuestionModel> Questions { get; set; }

    public List<FeedbackModel> Feedbacks { get; set; }
}

public class OwnerDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Avatar { get; set; }
    public double Rate { get; set; }
    public bool IsAdmin { get; set; }
}
