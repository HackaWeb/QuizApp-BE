using QuizApp.Contracts.Rest.Models;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;

namespace QuizApp.Application.Profile;

public class QuizProfile : AutoMapper.Profile
{
    public QuizProfile()
    {
        CreateMap<QuizApp.Domain.Models.Quiz, QuizModel>();
        CreateMap<QuizApp.Domain.Models.Quiz, QuizDto>();
        CreateMap<QuizApp.Domain.Models.Question, QuestionModel>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (QuestionType)src.Type));
        CreateMap<QuizApp.Domain.Models.AnswerOption, AnswerOptionModel>();
        CreateMap<QuizApp.Domain.Models.Feedback, FeedbackModel>();
        CreateMap<QuizApp.Domain.Models.Quiz, QuizModelWithOwner>()
            .ForMember(x => x.Owner, src => src.Ignore());

        CreateMap<QuizApp.Domain.Models.Question, QuestionWithOptions>()
            .ForMember(x => x.Options, src => src.MapFrom(x => x.ChoiceOptions));
        CreateMap<QuizApp.Domain.Models.AnswerOption, AnonymousOptionDto>();
        CreateMap<QuizApp.Domain.Models.User, UserDto>();
        CreateMap<QuizApp.Domain.Models.Question, QuestionDto>();
    }
}
