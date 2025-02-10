using AutoMapper;
using MediatR;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Contracts.Rest.Responses;
using QuizApp.Infrastructure;

namespace QuizApp.Application.Handlers;

public class GetAllQuizzesHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllQuizzesRequest, GetAllQuizzesResponse>
{
    public async Task<GetAllQuizzesResponse> Handle(GetAllQuizzesRequest request, CancellationToken cancellationToken)
    {
        var domainQuizzes = await unitOfWork.QuizRepository.GetAllAsync();
        var quizModels = mapper.Map<List<QuizModel>>(domainQuizzes);

        return new GetAllQuizzesResponse(quizModels);
    }
}
