using QuizApp.Domain.Models;

namespace QuizApp.Infrastructure.Specifications;
public class QuizSpecification : Specification<Quiz>
{
    public QuizSpecification(Guid userId, bool isReadOnly = false) 
        : base(q => q.OwnerId == userId, isReadOnly)
    {
        AddInclude(x => x.Questions);
    }
}
