namespace ChatApp.DTO;

public class ApiError
{
    public int StatusCode { get; set; }
    public List<KeyValuePair<string, string>> Errors { get; set; }


    public ApiError(int statusCode, List<KeyValuePair<string, string>>? errors = null)
    {
        StatusCode = statusCode;
        Errors = errors ?? new();
    }
}