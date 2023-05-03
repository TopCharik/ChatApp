namespace ChatApp.DTO;

public class ApiException : ApiError
{

    public ApiException(int statusCode, string? message = null, string? details = null)
        : base(statusCode)
    {
        Errors.Add( new KeyValuePair<string, string>("Message", message ?? "Internal server error") );
        if (details != null)
        {
            Errors.Add(new KeyValuePair<string, string>("Details", message ?? details));
        }
    }

}