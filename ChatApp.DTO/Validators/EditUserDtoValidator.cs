using System.Text.RegularExpressions;
using FluentValidation;

namespace ChatApp.DTO.Validators;

public class EditUserDtoValidator : AbstractValidator<EditUserDto>
{
    public EditUserDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(256);
        RuleFor(x => x.LastName)
            .MinimumLength(2)
            .MaximumLength(256);
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.PhoneNumber)
            .Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("PhoneNumber not valid")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}