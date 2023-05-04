using ChatApp.DTO;

namespace ChatApp.BlazorServer.StateContainers;

public class CallState
{
    public CallUsernamesDto CallUsernames { get; set; }
    public bool IsReceiver { get; set; }
}