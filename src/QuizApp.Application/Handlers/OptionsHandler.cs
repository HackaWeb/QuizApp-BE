using AutoMapper;
using MediatR;
using QuizApp.Contracts.Rest.Models;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;
public class CreateOptionHandler : IRequestHandler<CreateOptionRequest, OptionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOptionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OptionDto> Handle(CreateOptionRequest request, CancellationToken cancellationToken)
    {
        var option = new AnswerOption
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            IsCorrect = request.IsCorrect,
            QuestionId = request.QuestionId
        };

        await _unitOfWork.OptionsRepository.AddAsync(option);
        return _mapper.Map<OptionDto>(option);
    }
}

public class UpdateOptionHandler : IRequestHandler<UpdateOptionRequest, OptionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOptionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OptionDto> Handle(UpdateOptionRequest request, CancellationToken cancellationToken)
    {
        var option = await _unitOfWork.OptionsRepository.GetByIdAsync(request.OptionId);
        if (option == null) throw new KeyNotFoundException($"Option {request.OptionId} not found");

        if (!string.IsNullOrWhiteSpace(request.Title))
            option.Title = request.Title;
        if (request.IsCorrect.HasValue)
            option.IsCorrect = request.IsCorrect.Value;

        await _unitOfWork.SaveEntitiesAsync();
        return _mapper.Map<OptionDto>(option);
    }
}

public class GetOptionHandler : IRequestHandler<GetOptionRequest, OptionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOptionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OptionDto> Handle(GetOptionRequest request, CancellationToken cancellationToken)
    {
        var option = await _unitOfWork.OptionsRepository.GetByIdAsync(request.OptionId);
        if (option == null) throw new KeyNotFoundException($"Option {request.OptionId} not found");

        return _mapper.Map<OptionDto>(option);
    }
}

public class DeleteOptionHandler : IRequestHandler<DeleteOptionRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOptionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteOptionRequest request, CancellationToken cancellationToken)
    {
        var option = await _unitOfWork.OptionsRepository.GetByIdAsync(request.OptionId);
        if (option == null) throw new KeyNotFoundException($"Option {request.OptionId} not found");

        await _unitOfWork.OptionsRepository.DeleteAsync(option.Id);
    }
}

