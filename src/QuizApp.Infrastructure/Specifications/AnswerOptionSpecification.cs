using QuizApp.Domain.Models;
using System.Linq.Expressions;

namespace QuizApp.Infrastructure.Specifications;
public class AnswerOptionSpecification : Specification<AnswerOption>
{
    public AnswerOptionSpecification(Guid questionId, bool isReadOnly = false) 
        : base(o => o.QuestionId == questionId, isReadOnly)
    {
    }
}
