using ChatApp.Core.Entities.AppUserAggregate;

public class CallParticipants
{
    public AppUser CallInitiator { get; set; }
    public AppUser CallReceiver { get; set; }
}