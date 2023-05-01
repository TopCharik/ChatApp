using FluentValidation;

namespace ChatApp.DTO.Validators;

public class NewChatDtoValidator : AbstractValidator<NewChatDto>
{
    public NewChatDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(128);
        RuleFor(x => x.ChatLink)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(128);
    }
}