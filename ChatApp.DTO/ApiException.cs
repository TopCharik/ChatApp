namespace ChatApp.DTO;

public class ApiException : ApiError
{

    public ApiException(int statusCode, string? message = null, string? details = null)
        : base(statusCode)
    {
        Errors.Add("message", message ?? "Server error");
        if (details != null)
        {
            Errors.Add("details", details);
        }
    }

}