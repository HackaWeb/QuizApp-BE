using QuizApp.Domain.Models;

namespace QuizApp.Infrastructure.Specifications;
public class QuizSpecification : Specification<Quiz>
{
    public QuizSpecification(Guid? userId = null, Guid? quizId = null, bool isReadOnly = false)
        : base(q =>
            (!userId.HasValue || q.OwnerId == userId.Value) &&
            (!quizId.HasValue || q.Id == quizId.Value),
            isReadOnly)
    {
        AddInclude(x => x.Questions);
    }
}
