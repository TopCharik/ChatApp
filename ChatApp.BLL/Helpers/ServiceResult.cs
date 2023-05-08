namespace ChatApp.BLL.Helpers;

public class ServiceResult
{
    public bool Succeeded => Errors == null;
    public List<KeyValuePair<string, string>>? Errors { get; }
    
    public ServiceResult( List<KeyValuePair<string, string>>? errors = null)
    {
        Errors = errors;
    }
}

public class ServiceResult<T>
{
    public ServiceResult(List<KeyValuePair<string, string>>? errors)
    {
        Errors = errors;
    }

    public ServiceResult(T value)
    {
        Value = value;
    }

    public bool Succeeded => Value != null;
    public List<KeyValuePair<string, string>>? Errors { get; private set; }
    public T? Value { get; }
}