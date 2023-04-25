namespace ChatApp.BLL.Helpers;

public class ServiceResult
{
    public ServiceResult(Dictionary<string, string>? errors = null)
    {
        Succeeded = errors == null;
        Errors = errors;
    }

    public bool Succeeded { get; private set; }
    public Dictionary<string, string>? Errors { get; private set; }
}