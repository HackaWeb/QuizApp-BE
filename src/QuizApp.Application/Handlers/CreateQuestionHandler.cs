using AutoMapper;
using MediatR;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;

public class CreateQuestionHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateQuestionRequest, QuestionDto>
{
    public async Task<QuestionDto> Handle(CreateQuestionRequest request, CancellationToken cancellationToken)
    {
        var question = new Question
        {
            Id = request.QuestionId,
            QuizId = request.QuizId,
            Text = request.Text,
            Type = (Domain.QuestionType)request.Type
        };

        await unitOfWork.QuestionRepository.AddAsync(question);
        return mapper.Map<QuestionDto>(question);
    }
}
