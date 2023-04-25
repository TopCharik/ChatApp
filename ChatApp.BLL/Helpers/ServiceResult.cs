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

public class ServiceResult<T>
{
    public ServiceResult(Dictionary<string, string>? errors)
    {
        Succeeded = false;
        Errors = errors;
    }

    public ServiceResult(T value)
    {
        Succeeded = true;
        Value = value;
    }

    public bool Succeeded { get; private set; }
    public Dictionary<string, string>? Errors { get; private set; }
    public T? Value { get; private set; }
}