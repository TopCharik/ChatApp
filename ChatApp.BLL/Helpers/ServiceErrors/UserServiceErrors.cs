namespace ChatApp.BLL.Helpers.ServiceErrors;

public class UserServiceErrors
{
    public static List<KeyValuePair<string, string>> USER_NOT_FOUND_BY_USERNAME = new()
    {
        new("Get User By Username failed", "user with this username not found."),
    };
    public static List<KeyValuePair<string, string>> USER_NOT_FOUND_BY_CONNECTIONID = new()
    {
        new("Get User By ConnectionId failed", "user not found by ConnectionId"),
    };
    public static List<KeyValuePair<string, string>> USER_IS_ALREADY_IN_CALL = new()
    {
        new("Set In Call failed", "user is already in call"),
    };
}