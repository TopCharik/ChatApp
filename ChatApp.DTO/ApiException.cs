namespace ChatApp.DTO;

public class ApiException : ApiError
{
    public string Details { get; set; }

    public ApiException(int statusCode, string? message = null, string? details = null)
        : base(statusCode, message)
    {
        Details = details ?? string.Empty;
    }

}