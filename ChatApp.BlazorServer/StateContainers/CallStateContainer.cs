namespace ChatApp.BlazorServer.StateContainers;

public class CallStateContainer
{
    private CallState? _currentCall;
    public CallState? CurrentCall
    {
        get => _currentCall;
        set
        {
            _currentCall = value;
            NotifyStateChanged();
        }
    }

    public bool IsInCall => CurrentCall != null;
    
    public string? currentUsername => CurrentCall is {IsReceiver: true} 
        ? CurrentCall?.CallUsernames.callReceiverUsername 
        : CurrentCall?.CallUsernames.callInitiatorUsername;
    
    public string? remoteUsername => CurrentCall is {IsReceiver: true} 
        ? CurrentCall?.CallUsernames.callInitiatorUsername 
        : CurrentCall?.CallUsernames.callReceiverUsername;
    
    public event Action? OnStateChange;
    private void NotifyStateChanged() => OnStateChange?.Invoke();
}