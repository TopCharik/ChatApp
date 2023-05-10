namespace ChatApp.BLL.Helpers.ServiceErrors;

public class MessageServiceErrors
{
    public static  List<KeyValuePair<string, string>> CHAT_NOT_FOUND = new()
    {
        new ("Get messages failed", "Chat not found."),
    };
    public static  List<KeyValuePair<string, string>> USER_CANT_READ_MESSAGES_FROM_THIS_CHAT = new()
    {
        new ("Get messages failed", "This is a private chat. Only participants can read messages."),
    };
    public static  List<KeyValuePair<string, string>> USER_IS_NOT_A_MEMBER_OF_THIS_CONVERSATION = new()
    {
        new ("Message creation failed", "User is not a member of this conversation."),
    };
    public static  List<KeyValuePair<string, string>> USER_CANT_WRITE_MESSAGES_IN_THIS_CHAT = new()
    {
        new ("Message creation failed", "User can't write messages in this chat."),
    };
    
    public static  List<KeyValuePair<string, string>> USER_IS_MUTED_UNTIL(DateTime? mutedUntil) => new()
    {
        new ("Message creation failed", $"User muted until {mutedUntil}."),
    };
}