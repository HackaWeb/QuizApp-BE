using FluentValidation;
using QuizApp.Contracts.Rest.Requests;

namespace QuizApp.API.Validation;

public class UpdateUserProfileRequestValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileRequestValidator()
    {
    }
}
