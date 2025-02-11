using AutoMapper;
using MediatR;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.Application.Handlers;

public class UpdateQuizHandler : IRequestHandler<UpdateQuizRequest, QuizDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateQuizHandler(IUnitOfWork quizRepository, IMapper mapper)
    {
        _unitOfWork = quizRepository;
        _mapper = mapper;
    }

    public async Task<QuizDto> Handle(UpdateQuizRequest request, CancellationToken cancellationToken)
    {
        var quiz = await _unitOfWork.QuizRepository.GetByIdAsync(request.QuizId);
        if (quiz == null) throw new KeyNotFoundException($"Quiz {request.QuizId} not found");

        if (!string.IsNullOrWhiteSpace(request.Title))
            quiz.Title = request.Title;
        if (!string.IsNullOrWhiteSpace(request.Description))
            quiz.Description = request.Description;
        if (request.Duration.HasValue)
            quiz.Duration = request.Duration.Value;

        await _unitOfWork.SaveEntitiesAsync();

        var dto = _mapper.Map<QuizDto>(quiz);
        return dto;
    }
}