using FluentValidation;

namespace ChatApp.DTO.Validators;

public class NewMessageDtoValidator : AbstractValidator<NewMessageDto>
{
    public NewMessageDtoValidator()
    {
        RuleFor(x => x.MessageText)
            .NotEmpty();
    }
}