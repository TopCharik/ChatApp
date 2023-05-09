namespace ChatApp.BLL.Helpers.ServiceErrors;

public class AvatarServiceErrors
{
    public static  List<KeyValuePair<string, string>> CHAT_WITH_THIS_LINK_DOESNT_EXIST =  new()
    {
        new ("Avatar upload failed", "Chat with this link doesn't exist."),
    }; 
    
    public static  List<KeyValuePair<string, string>> GIVED_USER_DONT_HAVE_PERMISSION_FOR_UPLOAD_AVATAR =  new()
    {
        new ("Avatar upload failed", "You don't have permission for upload avatar to this chat"),
    }; 
}