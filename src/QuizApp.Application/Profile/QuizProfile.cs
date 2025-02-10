using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Models.Quiz;

namespace QuizApp.Application.Profile;

public class QuizProfile : AutoMapper.Profile
{
    public QuizProfile()
    {
        CreateMap<QuizApp.Domain.Models.Quiz, QuizModel>();
        CreateMap<QuizApp.Domain.Models.Question, QuestionModel>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (QuestionType)src.Type));
        CreateMap<QuizApp.Domain.Models.AnswerOption, AnswerOptionModel>();
    }
}
