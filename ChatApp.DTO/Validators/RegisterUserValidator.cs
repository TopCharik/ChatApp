using System.Text.RegularExpressions;
using FluentValidation;

namespace ChatApp.DTO.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterAppUserDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(64);
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(256);
        RuleFor(x => x.LastName)
            .MinimumLength(2)
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Your password length must be at least 8 characters.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
        RuleFor(x => x.Birthday)
            .Must(x => x.Value < DateTime.Now).WithMessage("Birthday can't be in the future")
            .Must(x => x.Value > DateTime.Now.AddYears(-120)).WithMessage("Age can't be more than 120 years")
            .When(x => x.Birthday != null);
        RuleFor(x => x.PhoneNumber)
            .Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"))
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("PhoneNumber not valid. Valid phone example: +38-050-193-5524");
    }
}