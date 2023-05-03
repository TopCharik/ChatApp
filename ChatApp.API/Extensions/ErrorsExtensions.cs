using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.API.Extensions;

public static class ErrorsExtensions
{
    public static List<KeyValuePair<string, string>> ToKeyValuePairs(this List<ValidationFailure> validationFailures)
    {
        return validationFailures
            .Select(x => new KeyValuePair<string, string>(x.PropertyName, x.ErrorMessage))
            .ToList();
    }

    public static List<KeyValuePair<string, string>> ToKeyValuePairs(this IEnumerable<IdentityError> validationFailures)
    {
        return validationFailures
            .Select(x => new KeyValuePair<string, string>(x.Code, x.Description))
            .ToList();
    }
}