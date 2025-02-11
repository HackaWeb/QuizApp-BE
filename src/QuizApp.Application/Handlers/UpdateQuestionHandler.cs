using AutoMapper;
using MediatR;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;

public class UpdateQuestionHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateQuestionRequest, QuestionDto>
{
    public async Task<QuestionDto> Handle(UpdateQuestionRequest request, CancellationToken cancellationToken)
    {
        var question = await unitOfWork.QuestionRepository.GetByIdAsync(request.QuestionId);
        if (question == null) throw new KeyNotFoundException($"Question {request.QuestionId} not found");

        if (!string.IsNullOrWhiteSpace(request.Text))
            question.Text = request.Text;
        if (request.Type.HasValue)
            question.Type = (Domain.QuestionType)request.Type.Value;

        await unitOfWork.SaveEntitiesAsync();
        return mapper.Map<QuestionDto>(question);
    }
}
