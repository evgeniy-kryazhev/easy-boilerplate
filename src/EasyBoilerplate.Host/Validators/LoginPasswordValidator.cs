using EasyBoilerplate.Host.ViewModels;
using FluentValidation;

namespace EasyBoilerplate.Host.Validators;

public class LoginPasswordValidator : AbstractValidator<LoginPasswordViewModel>
{
    public LoginPasswordValidator()
    {
        RuleFor(request => request.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();

        RuleFor(request => request.Password)
            .NotEmpty()
            .NotNull();
    }
}
