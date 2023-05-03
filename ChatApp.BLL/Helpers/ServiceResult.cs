namespace ChatApp.BLL.Helpers;

public class ServiceResult
{
    public bool Succeeded { get; private set; }
    public List<KeyValuePair<string, string>>? Errors { get; private set; }
    
    public ServiceResult( List<KeyValuePair<string, string>>? errors = null)
    {
        Succeeded = errors == null;
        Errors = errors;
    }
}

public class ServiceResult<T>
{
    public ServiceResult(List<KeyValuePair<string, string>>? errors)
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
    public List<KeyValuePair<string, string>>? Errors { get; private set; }
    public T? Value { get; private set; }
}