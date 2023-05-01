using FluentValidation;

namespace ChatApp.DTO.Validators;

public class NewUsernameDtoValidator : AbstractValidator<NewUsernameDto>
{
    public NewUsernameDtoValidator()
    {
        RuleFor(x => x.NewUsername)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(64);
    }
}