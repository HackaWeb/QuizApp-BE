using AutoMapper;
using MediatR;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.Application.Handlers;
public class GetQuestionHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetQuestionRequest, QuestionDto>
{
    public async Task<QuestionDto> Handle(GetQuestionRequest request, CancellationToken cancellationToken)
    {
        var question = await unitOfWork.QuestionRepository.GetByIdAsync(request.QuestionId);
        if (question == null) throw new KeyNotFoundException($"Question {request.QuestionId} not found");

        return mapper.Map<QuestionDto>(question);
    }
}

public class DeleteQuestionHandler : IRequestHandler<DeleteQuestionRequest>
{
    private readonly IUnitOfWork unitOfWork;

    public DeleteQuestionHandler(IUnitOfWork questionRepository)
    {
        unitOfWork = questionRepository;
    }

    public async Task Handle(DeleteQuestionRequest request, CancellationToken cancellationToken)
    {
        var question = await unitOfWork.QuestionRepository.GetByIdAsync(request.QuestionId);
        if (question == null) throw new KeyNotFoundException($"Question {request.QuestionId} not found");

        await unitOfWork.QuestionRepository.DeleteAsync(question.Id);
    }
}
