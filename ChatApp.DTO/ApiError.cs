namespace ChatApp.DTO;

public class ApiError
{
    public int StatusCode { get; set; }
    public Dictionary<string, string> Errors { get; set; }


    public ApiError(int statusCode, Dictionary<string, string>? errors = null)
    {
        StatusCode = statusCode;
        Errors = errors ?? new();
    }
}