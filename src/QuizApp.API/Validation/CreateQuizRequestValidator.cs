using FluentValidation;
using QuizApp.Contracts.Rest.Models.Quiz;
using QuizApp.Contracts.Rest.Requests;

namespace QuizApp.API.Validation;

public class CreateQuizRequestValidator : AbstractValidator<CreateQuizRequest>
{
    public CreateQuizRequestValidator()
    {
        RuleFor(q => q.Quiz.Title)
            .NotEmpty().WithMessage("The quiz should have a title.")
            .MaximumLength(100).WithMessage("The header is too long (max. 100 characters).");

        RuleFor(q => q.Quiz.Description)
            .MaximumLength(500).WithMessage("The description is too long (max. 500 characters).");

        RuleFor(q => (int)q.Quiz.Duration)
            .GreaterThan(0).WithMessage("There should be at least 1 question in the test.");

        RuleFor(q => q.Quiz.Questions)
            .NotEmpty().WithMessage("⚠️ ")
            .ForEach(q => q.SetValidator(new CreateQuestionModelValidator()));
    }
}

public class CreateQuestionModelValidator : AbstractValidator<CreateQuestionModel>
{
    public CreateQuestionModelValidator()
    {
        RuleFor(q => q.QuestionId)
            .NotEmpty().WithMessage("QuestionId is required.");

        RuleFor(q => q.Title)
            .NotEmpty().WithMessage("The question should have a headline.")
            .MaximumLength(200).WithMessage("The header is too long (max. 200 characters).");
    }
}
