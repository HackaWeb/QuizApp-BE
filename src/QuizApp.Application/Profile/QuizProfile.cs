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
        CreateMap<QuizApp.Domain.Models.Feedback, FeedbackModel>();
        CreateMap<QuizApp.Domain.Models.Quiz, QuizModelWithOwner>()
            .ForMember(x => x.Owner, src => src.Ignore());

        CreateMap<QuizApp.Domain.Models.Question, QuestionWithOptions>();
        CreateMap<QuizApp.Domain.Models.AnswerOption, AnonymousOptionDto>();
    }
}
