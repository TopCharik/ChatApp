namespace ChatApp.BLL.Helpers.ServiceErrors;

public class ChatServiceErrors
{
    public static  List<KeyValuePair<string, string>> USER_IS_ALLREADY_MEMBER_OF_THIS_CHAT = new()
    {
        new ("Joining chat failed", "User is already member of this chat."),
    };
    
    public static  List<KeyValuePair<string, string>> USER_CANT_JOIN_PRIVATE_CHAT = new()
    {
        new ("Joining chat failed", "This is a private chat. User can't join this chat."),
    };
    
    public static  List<KeyValuePair<string, string>> USER_IS_NOT_A_MEMBER_OF_THIS_CHAT = new()
    {
        new ("Chat leave failed", "User is not a member of this chat."),
    };
    
    public static  List<KeyValuePair<string, string>>  CHAT_WITH_THIS_LINK_ALLREADY_EXIST = new()
    {
        new ("Chat creation failed", "Chat with this link already exist."),
    };
    
    public static  List<KeyValuePair<string, string>> CHAT_WITH_THIS_LINK_DOESNT_EXIST = new()
    {
        new ("Chat not found", "Chat with this link doesn't exist."),
    };
    public static  List<KeyValuePair<string, string>> USER_CANT_ADD_AVATAR_TO_THIS_CHAT = new()
    {
        new ("Avatar upload failed", "You don't have permission for upload avatar to this chat"),
    };
}