namespace ChatApp.DTO;

public class ApiError
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }


    public ApiError(int statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultTitle(statusCode);
        Errors = new();
    }


    private string GetDefaultTitle(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Unknown Error",
        };
    }
}