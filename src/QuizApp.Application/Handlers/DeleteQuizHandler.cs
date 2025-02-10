using MediatR;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Specifications;

namespace QuizApp.Application.Handlers;

public class DeleteQuizHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteQuizRequest>
{
    public async Task Handle(DeleteQuizRequest request, CancellationToken cancellationToken)
    {
        await unitOfWork.QuizRepository.DeleteAsync(request.quizId);
    }
}
