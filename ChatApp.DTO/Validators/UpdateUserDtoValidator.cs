using System.Text.RegularExpressions;
using FluentValidation;

namespace ChatApp.DTO.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
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
            .MinimumLength(6).WithMessage("PhoneNumber must not be less than 6 characters.")
            .MaximumLength(20).WithMessage("PhoneNumber must not exceed 20 characters.")
            .Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("PhoneNumber not valid");
    }
}