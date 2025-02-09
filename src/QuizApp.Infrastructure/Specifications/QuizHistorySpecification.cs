using QuizApp.Domain.Models;
using System.Linq.Expressions;

namespace QuizApp.Infrastructure.Specifications;

public class QuizHistorySpecification : Specification<QuizHistory>
{
    public QuizHistorySpecification(Guid userId, bool isReadOnly = false) 
        : base(h => h.UserId == userId, isReadOnly)
    {
        AddInclude(x => x.Quiz);
    }
}
