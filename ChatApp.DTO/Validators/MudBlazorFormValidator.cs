using FluentValidation;

namespace ChatApp.DTO.Validators;

public class MudBlazorFormValidator<T>
{
    private readonly IValidator _validator;

    public MudBlazorFormValidator(IValidator validator)
    {
        _validator = validator;
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var context = ValidationContext<T>.CreateWithOptions((T) model, x => x.IncludeProperties(propertyName));
        var result = await _validator.ValidateAsync(context);
        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}