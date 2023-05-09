using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.DAL.App.AppContext;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.UnitTests;

public class FakeData
{
    public static List<AppUser> FakeUsers => _fakeUsers;
    public static List<Avatar> FakeAvatars => _fakeAvatars;
    public static List<Conversation> FakeConversations => _fakeConversations;
    public static List<ChatInfo> FakeChatInfos => _fakeChatInfos;
    public static List<Participation> FakeParticipations => _fakeParticipations;
    public static List<Message> FakeMessages => _fakeMessages;
    public static List<ChatInfoView> FakeChatInfoViews => _fakeChatInfoViews;

    public static async Task<AppDbContext> GetDefaultAppDbContext()
    {
        if (_defaultAppDbContext == null)
        {
            _defaultAppDbContext = await SetUpAppDbContextAsync();
        }

        return _defaultAppDbContext;
    }

    private static AppDbContext? _defaultAppDbContext;

    public static async Task<AppDbContext> SetUpAppDbContextAsync(
        List<AppUser>? users = null,
        List<Avatar>? avatars = null,
        List<Conversation>? conversations = null,
        List<ChatInfo>? chatInfos = null,
        List<Participation>? participations = null,
        List<Message>? messages = null,
        List<ChatInfoView>? chatInfoViews = null
    )
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: new Guid().ToString())
            .Options;
        var context = new AppDbContext(options);

        await context.AddRangeAsync(users ?? FakeUsers);
        await context.AddRangeAsync(avatars ?? FakeAvatars);
        await context.AddRangeAsync(conversations ?? FakeConversations);
        await context.AddRangeAsync(chatInfos ?? FakeChatInfos);
        await context.AddRangeAsync(participations ?? FakeParticipations);
        await context.AddRangeAsync(messages ?? FakeMessages);
        await context.AddRangeAsync(chatInfoViews ?? FakeChatInfoViews);
        await context.SaveChangesAsync();
        return context;
    }

    private static List<AppUser> _fakeUsers = new()
    {
        new()
        {
            Id = "6247B812-256F-4E98-8A77-8FEA1C67342F",
            FirstName = "David",
            LastName = "Smith",
            UserName = "DavidSmith",
            NormalizedUserName = "DAVIDSMITH",
            Email = "davidsmith@example.com",
            NormalizedEmail = "DAVIDSMITH@EXAMPLE.COM",
            PhoneNumber = "555-123-4567",
        },
        new()
        {
            Id = "00DA31F8-0619-4593-A89B-A39AA3FB88E7",
            FirstName = "David",
            LastName = "Smith",
            UserName = "DavidSmith",
            NormalizedUserName = "DAVIDSMITH",
            Email = "davidsmith@example.com",
            NormalizedEmail = "DAVIDSMITH@EXAMPLE.COM",
            PhoneNumber = "555-123-4567",
            CallHubConnectionId = "735AD785D8CD4723989A87B6DF7074EF",
        },
        new()
        {
            Id = "661ABAAB-DA8F-46B9-B796-52A258C591BF",
            FirstName = "David",
            LastName = "Smith",
            UserName = "DavidSmith",
            NormalizedUserName = "DAVIDSMITH",
            Email = "davidsmith@example.com",
            NormalizedEmail = "DAVIDSMITH@EXAMPLE.COM",
            PhoneNumber = "555-123-4567",
        },
        new()
        {
            Id = "F6A8A0DC-DB3F-449A-B95F-9D63C03758A1",
            FirstName = "John",
            LastName = "Doe",
            UserName = "JohnD05DE7939F274AB68853D0D9282FA284Doe",
            NormalizedUserName = "JOHNDOED05DE7939F274AB68853D0D9282FA284",
            Email = "johndoe@example.com",
            NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
            PhoneNumber = "+12-345-6789",
        },
        new()
        {
            Id = "2AD1DA00-7808-42EC-BADD-7318882ABD49",
            FirstName = "JaneD05DE7939F274AB68853D0D9282FA284",
            LastName = "Doe",
            UserName = "janedoe",
            NormalizedUserName = "JANEDOE",
            Email = "janedoe@example.com",
            NormalizedEmail = "JANEDOE@EXAMPLE.COM",
            PhoneNumber = "098-765-4321",
        },
        new()
        {
            Id = "456BC3A3-8B95-45EA-96CE-36802456D77E",
            FirstName = "Mary",
            LastName = "Doe",
            UserName = "MaryD",
            NormalizedUserName = "MARYD",
            Email = "maryjohnsonD05DE7939F274AB68853D0D9282FA284@example.com",
            NormalizedEmail = "MARYJOHNSOND05DE7939F274AB68853D0D9282FA284@EXAMPLE.COM",
            PhoneNumber = "555-987-6543",
        },
        new()
        {
            Id = "B4CB6F4D-76ED-4BEC-9C22-8A51D051A720",
            FirstName = "Mary",
            LastName = "Doe",
            UserName = "MaryD",
            NormalizedUserName = "MARYD",
            Email = "maryjohnson@example.com",
            NormalizedEmail = "MARYJOHNSON@EXAMPLE.COM",
            PhoneNumber = "555-945-6789",
        },
        new()
        {
            Id = "0D64737E-F602-44D8-9B0D-83D2E2C7FEBA",
            FirstName = "Robert",
            LastName = "Brown",
            UserName = "RobertB",
            NormalizedUserName = "ROBERTB",
            Email = "robertbrown@example.com",
            NormalizedEmail = "ROBERTBROWN@EXAMPLE.COM",
            PhoneNumber = "555-545-6893",
        },
        new()
        {
            Id = "E72B33C2-DF13-4D03-ABEE-932A529AB1B8",
            FirstName = "Alice",
            LastName = "Smith",
            UserName = "alice.smith",
            NormalizedUserName = "ALICE.SMITH",
            Email = "alice.smith@example.com",
            NormalizedEmail = "ALICE.SMITH@EXAMPLE.COM",
            PhoneNumber = "111-245-6893",
        },
        new()
        {
            Id = "EC94280C-5E90-4D4C-9214-49799B4E5C92",
            FirstName = "Bob",
            LastName = "Smith",
            UserName = "bob.Smith",
            NormalizedUserName = "BOB.SMITH",
            Email = "bob.johnson@example.com",
            NormalizedEmail = "BOB.JOHNSON@EXAMPLE.COM",
            PhoneNumber = "444-545-6893",
        },
        new()
        {
            Id = "2F3F3D50-1271-4A16-BC26-9A726155E2B2",
            FirstName = "Carol",
            LastName = "Davis",
            UserName = "cdavis",
            NormalizedUserName = "CDAVIS",
            Email = "cdavis@example.com",
            NormalizedEmail = "CDAVIS@EXAMPLE.COM",
            PhoneNumber = "777-888-9999",
        },
        new()
        {
            Id = "FE74C855-04F5-44E5-9B04-BE69F0ED59E7",
            FirstName = "Carol",
            LastName = "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
            UserName = "cdavis",
            NormalizedUserName = "CDAVIS",
            Email = "cdavis@example.com",
            NormalizedEmail = "CDAVIS@EXAMPLE.COM",
            PhoneNumber = "777-888-9999",
        },
        new()
        {
            Id = "A2B6FFA7-9320-466C-AC41-077F9463F570",
            FirstName = "David",
            LastName = "ZZZZZZ",
            UserName = "dbrown",
            NormalizedUserName = "DBROWN",
            Email = "dbrown@example.com",
            NormalizedEmail = "DBROWN@EXAMPLE.COM",
            PhoneNumber = "222-333-4444",
        },
        new()
        {
            Id = "482B198F-1064-467F-B053-F9CEE91EF34E",
            FirstName = "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
            LastName = "Brown",
            UserName = "dbrown",
            NormalizedUserName = "DBROWN",
            Email = "dbrown@example.com",
            NormalizedEmail = "DBROWN@EXAMPLE.COM",
            PhoneNumber = "222-333-4444",
        },
        new()
        {
            Id = "24CBCD2A-D458-4502-8A3D-A47A90F20D2C",
            FirstName = "ZZZZZZZZZZZZZZZZZZZZ",
            LastName = "Taylor",
            UserName = "etaylor",
            NormalizedUserName = "ETAYLOR",
            Email = "etaylor@example.com",
            NormalizedEmail = "ETAYLOR@EXAMPLE.COM",
            PhoneNumber = "555-666-7777",
        },
        new()
        {
            Id = "5DE50785-60C3-4B5C-8EE8-6FFDBACEB671",
            FirstName = "David",
            LastName = "Brown",
            UserName = "!!!!!!!!!!!!",
            NormalizedUserName = "!!!!!!!!!!!!",
            Email = "dbrown@example.com",
            NormalizedEmail = "DBROWN@EXAMPLE.COM",
            PhoneNumber = "222-333-4444",
        },
        new()
        {
            Id = "99078870-7BAB-44F6-97D8-340EC0EC2FFC",
            FirstName = "Emily",
            LastName = "Taylor",
            UserName = "ZZZZZZ",
            NormalizedUserName = "ZZZZZZ",
            Email = "etaylor@example.com",
            NormalizedEmail = "ETAYLOR@EXAMPLE.COM",
            PhoneNumber = "555-666-7777",
        },
        new()
        {
            Id = "2CFDB80C-BBB3-4633-B47F-5F081CD7496C",
            FirstName = "David",
            LastName = "Brown",
            UserName = "dbrown",
            NormalizedUserName = "DBROWN",
            Email = "!!!!!!!!!!!!!!!!@example.com",
            NormalizedEmail = "!!!!!!!!!!!!!!!!@EXAMPLE.COM",
            PhoneNumber = "222-333-4444",
        },
        new()
        {
            Id = "F8A3D859-28AB-40BE-BE19-9935A47F0EAE",
            FirstName = "Emily",
            LastName = "Taylor",
            UserName = "etaylor",
            NormalizedUserName = "ETAYLOR",
            Email = "ZZZZZ@example.com",
            NormalizedEmail = "ZZZZZ@EXAMPLE.COM",
            PhoneNumber = "555-666-7777",
        },
        new()
        {
            Id = "382A1E62-1A84-4722-8E53-F55ECBCBA3C3",
            FirstName = "David",
            LastName = "Brown",
            UserName = "dbrown",
            NormalizedUserName = "DBROWN",
            Email = "dbrown@example.com",
            NormalizedEmail = "DBROWN@EXAMPLE.COM",
            PhoneNumber = "!!!!!!!!!!11-111-1111",
        },
        new()
        {
            Id = "38603CBC-C704-41C0-AA3E-D8B0E8A32F38",
            FirstName = "Emily",
            LastName = "Taylor",
            UserName = "etaylor",
            NormalizedUserName = "ETAYLOR",
            Email = "etaylor@example.com",
            NormalizedEmail = "ETAYLOR@EXAMPLE.COM",
            PhoneNumber = "ZZZZZZZZZZ999-999-9999",
        },
        new()
        {
            Id = "F8E54710-0C41-4E64-99F8-8B9B9B31E981",
            FirstName = "David",
            LastName = "Brown",
            UserName = "dbrown",
            NormalizedUserName = "DBROWN",
            Email = "dbrown@example.com",
            NormalizedEmail = "DBROWN@EXAMPLE.COM",
            PhoneNumber = "222-333-4444",
        },
        new()
        {
            Id = "EF6A36C6-461A-427B-8A01-0E9B7D0902A6",
            FirstName = "Emily",
            LastName = "Taylor",
            UserName = "etaylor",
            NormalizedUserName = "ETAYLOR",
            Email = "etaylor@example.com",
            NormalizedEmail = "ETAYLOR@EXAMPLE.COM",
            PhoneNumber = "555-666-7777",
        },
        new()
        {
            Id = "0C60C495-FB2F-47A9-B9CE-0A9E98DD16A6",
            FirstName = "John",
            LastName = "Doe",
            UserName = "John0A9BB93B62054848A048AB765E518A46Doe",
            NormalizedUserName = "JOHN0A9BB93B62054848A048AB765E518A46DOE",
            Email = "johndoe@example.com",
            NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
            PhoneNumber = "+12-345-6789",
        },
        new()
        {
            Id = "6ABF1ADE-5B2C-4967-8D2F-000FC30FFA6E",
            FirstName = "Jane",
            LastName = "Doe",
            UserName = "janedoe0A9BB93B62054848A048AB765E518A46",
            NormalizedUserName = "JANEDOE0A9BB93B62054848A048AB765E518A46",
            Email = "janedoe@example.com",
            NormalizedEmail = "JANEDOE@EXAMPLE.COM",
            PhoneNumber = "098-765-4321",
        },
        new()
        {
            Id = "728E83DD-17AA-458A-B139-4CA62EFFDAD4",
            FirstName = "Rob4602B55B7D184AD58F8ABDBFCFD12AC2ert",
            LastName = "Brown",
            UserName = "RobertB",
            NormalizedUserName = "ROBERTB",
            Email = "robertbrown@example.com",
            NormalizedEmail = "ROBERTBROWN@EXAMPLE.COM",
            PhoneNumber = "555-555-5555",
        },
        new()
        {
            Id = "19F970F2-1924-4241-9EAB-CDD311721415",
            FirstName = "4602B55B7D184AD58F8ABDBFCFD12AC2Bob",
            LastName = "Smith",
            UserName = "bob.Smith",
            NormalizedUserName = "BOB.SMITH",
            Email = "bob.johnson@example.com",
            NormalizedEmail = "BOB.JOHNSON@EXAMPLE.COM",
            PhoneNumber = "444-555-6666",
        },
        new()
        {
            Id = "890AC95B-0CAA-42A7-8344-1640B44817E1",
            FirstName = "John",
            LastName = "D544A41B16E1F49A280A9CE557A0EBD35oe",
            UserName = "JohnDoe",
            NormalizedUserName = "JOHNDOE",
            Email = "johndoe@example.com",
            NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
            PhoneNumber = "+12-345-6789",
        },
        new()
        {
            Id = "4AB42DD8-D72F-4D73-A6CC-247C0C7FE2D9",
            FirstName = "Jane",
            LastName = "Doe544A41B16E1F49A280A9CE557A0EBD35",
            UserName = "janedoe",
            NormalizedUserName = "JANEDOE",
            Email = "janedoe@example.com",
            NormalizedEmail = "JANEDOE@EXAMPLE.COM",
            PhoneNumber = "098-765-4321",
        },
        new()
        {
            Id = "0BE8ACF4-60B9-4BBB-BD19-F9F95F62974F",
            FirstName = "Mary",
            LastName = "544A41B16E1F49A280A9CE557A0EBD35Doe",
            UserName = "MaryD",
            NormalizedUserName = "MARYD",
            Email = "maryjohnson@example.com",
            NormalizedEmail = "MARYJOHNSON@EXAMPLE.COM",
            PhoneNumber = "555-987-6543",
        },
        new()
        {
            Id = "E9A0D556-B6CA-4D02-97C9-97A16B4B19B2",
            FirstName = "John",
            LastName = "Doe",
            UserName = "JohnDoe",
            NormalizedUserName = "JOHNDOE",
            Email = "john8479664F638C4BB7AA390A055A3B918Ddoe@example.com",
            NormalizedEmail = "JOHN8479664F638C4BB7AA390A055A3B918DDOE@EXAMPLE.COM",
            PhoneNumber = "+12-345-6789",
        },
        new()
        {
            Id = "38B5F211-9869-4D5C-9FD8-E154E3168314",
            FirstName = "Jane",
            LastName = "Doe",
            UserName = "janedoe",
            NormalizedUserName = "JANEDOE",
            Email = "janedoe@example8479664F638C4BB7AA390A055A3B918D.com",
            NormalizedEmail = "JANEDOE@EXAMPLE8479664F638C4BB7AA390A055A3B918D.COM",
            PhoneNumber = "098-765-4321",
        },
        new()
        {
            Id = "4955F482-2B6B-41CA-94AA-BCBE78EE1CA6",
            FirstName = "Robert",
            LastName = "Brown",
            UserName = "RobertB",
            NormalizedUserName = "ROBERTB",
            Email = "robertbrown@example.com",
            NormalizedEmail = "ROBERTBROWN@EXAMPLE.COM",
            PhoneNumber = "555-7339BA628BF341DC897F03E2ABDE8725-6893",
        },
        new()
        {
            Id = "9D332CD6-2CFA-4759-BF77-788A7FDC5FD1",
            FirstName = "Alice",
            LastName = "Smith",
            UserName = "alice.smith",
            NormalizedUserName = "ALICE.SMITH",
            Email = "alice.smith@example.com",
            NormalizedEmail = "ALICE.SMITH@EXAMPLE.COM",
            PhoneNumber = "111-245-7339BA628BF341DC897F03E2ABDE8725",
        },
        new()
        {
            Id = "439362BF-1390-434B-838B-8733AB727134",
            FirstName = "Bob",
            LastName = "Smith",
            UserName = "bob.Smith",
            NormalizedUserName = "BOB.SMITH",
            Email = "bob.johnson@example.com",
            NormalizedEmail = "BOB.JOHNSON@EXAMPLE.COM",
            PhoneNumber = "7339BA628BF341DC897F03E2ABDE8725-545-6893",
        },
        new()
        {
            Id = "340E7DB9-1CBB-4B6D-84DD-DE95276E9AD7",
            FirstName = "John",
            LastName = "Doe",
            UserName = "B73FB9C10A42455991B90EB4F0262699",
            NormalizedUserName = "B73FB9C10A42455991B90EB4F0262699",
            Email = "johndoe@example.com",
            NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
            PhoneNumber = "+12-345-6789",
        },
        new()
        {
            Id = "57453247-3B50-4034-90C3-47473C333E40",
            FirstName = "John",
            LastName = "Doe",
            UserName = "JohnDoe",
            NormalizedUserName = "JOHNDOE",
            Email = "johndoe@example.com",
            NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
            PhoneNumber = "+12-345-6666",
        },
        new()
        {
            Id = "93F6E32F-FB4E-4A26-BA89-41437427C30D",
            FirstName = "Jane",
            LastName = "Doe",
            UserName = "janedoe",
            NormalizedUserName = "JANEDOE",
            Email = "janedoe@example.com",
            NormalizedEmail = "JANEDOE@EXAMPLE.COM",
            PhoneNumber = "098-765-4321",
        },
        new()
        {
            Id = "2D829992-8FB0-4800-98A2-0E4E429C6708",
            FirstName = "Jane",
            LastName = "Doe",
            UserName = "janedoe",
            NormalizedUserName = "JANEDOE",
            Email = "janedoe@example.com",
            NormalizedEmail = "JANEDOE@EXAMPLE.COM",
            PhoneNumber = "098-765-4321",
        },
    };

    private static List<Avatar> _fakeAvatars = new()
    {
        new()
        {
            Id = 87401,
            UserId = "436E26E8-45B8-4971-A81B-663A40A93893",
            DateSet = DateTime.Now,
            ImagePayload = "qwerty",
        },
        new()
        {
            Id = 25489,
            UserId = "2D829992-8FB0-4800-98A2-0E4E429C6708",
            DateSet = DateTime.Now,
            ImagePayload = "qwerty",
        },
        new()
        {
            Id = 236706,
            UserId = "71487432-92E3-49F9-8ECB-31FFD92FD581",
            DateSet = DateTime.Now,
            ImagePayload = "b8f8b974a159415192d0863f85617ef8",
        },
        new()
        {
            Id = 18645,
            ChatInfoId = 26481,
            DateSet = DateTime.Now,
            ImagePayload = "D34020D2-B3DC-4B5D-B0C4-8586F239D469",
        },
        new()
        {
            Id = 45682,
            ChatInfoId = 98762,
            ImagePayload = "84B3748352334877A3F77EE4D4BA2ED9",
            DateSet = DateTime.Today,
        },
        new()
        {
            Id = 438191,
            UserId = "93F6E32F-FB4E-4A26-BA89-41437427C30D",
            ImagePayload = "698EB7EAB6B4457891AB4CA431DF918E",
            DateSet = DateTime.Today,
        }
    };

    private static List<Conversation> _fakeConversations = new()
    {
        new()
        {
            Id = 1,
            ChatInfoId = 1,
        },
        new()
        {
            Id = 2,
            ChatInfoId = 2,
        },
        new()
        {
            Id = 87561,
            ChatInfoId = 26481,
        },
        new()
        {
            Id = 184623,
            ChatInfoId = 44343,
        },
        new()
        {
            Id = 553486,
            ChatInfoId = 137597,
        },
        new()
        {
            Id = 51348,
            ChatInfoId = 98762,
        },
        new()
        {
            Id = 4528,
            ChatInfoId = 88425,
        },
        new()
        {
            Id = 5588,
            ChatInfoId = 96241,
        },
        new()
        {
        Id = 25482,
        ChatInfoId = 4682195,
    },
    };

    private static List<ChatInfo> _fakeChatInfos = new()
    {
        new()
        {
            Id = 1,
            Title = "First Chat",
            ChatLink = "First_Chat",
            IsPrivate = false,
        },
        new()
        {
            Id = 2,
            Title = "Second Chat",
            ChatLink = "Second_Chat",
            IsPrivate = false,
        },
        new()
        {
            Id = 26481,
            Title = "Third Chat",
            ChatLink = "1AD31E854732405A9192519324462E34",
            IsPrivate = true,
        },
        new()
        {
            Id = 44343,
            Title = "Fourth Chat",
            ChatLink = "Fourth_Chat",
            IsPrivate = true,
        },
        new()
        {
            Id = 5,
            Title = "Fifth Chat",
            ChatLink = "Fifth_Chat",
            IsPrivate = false,
        },
        new()
        {
            Id = 137597,
            Title = "Include AppUser  test Chat",
            ChatLink = "Include_AppUser_test_Chat",
            IsPrivate = false,
        },
        new()
        {
            Id = 98762,
            Title = "Chat Title",
            ChatLink = "42D422C6304C41AB82EF14FA9283D973",
            IsPrivate = false,
        },
        new()
        {
            Id = 88425,
            Title = "Chat Title",
            ChatLink = "42D422C6304C41AB82EF14FA9283D973",
            IsPrivate = false,
        },
        new()
        {
            Id = 96241,
            Title = "Chat Title",
            ChatLink = "3DE73A90-0BB4-44AA-B6F9-BC2099F0CCB2",
            IsPrivate = false,
        },
        new()
        {
        Id = 4682195,
        Title = "Chat Title",
        ChatLink = "51491C65-9A5D-4050-908A-4439F33E4158",
        IsPrivate = false,
    },
    };

    private static List<Participation> _fakeParticipations = new()
    {
        new()
        {
            Id = 15468,
            AspNetUserId = "71487432-92E3-49F9-8ECB-31FFD92FD581",
            ConversationId = 553486,
            CanWriteMessages = true,
            CanMuteParticipants = false,
            CanDeleteMessages = false,
            CanAddParticipants = false,
            CanDeleteParticipants = false,
            CanChangePublicity = false,
            CanChangeChatAvatar = false,
            CanChangeChatTitle = false,
            CanSetPermissions = false,
            CanDeleteConversation = false,
        },
        new()
        {
            Id = 15444,
            AspNetUserId = "661ABAAB-DA8F-46B9-B796-52A258C591BF",
            ConversationId = 553486,
            CanWriteMessages = true,
            CanMuteParticipants = false,
            CanDeleteMessages = false,
            CanAddParticipants = false,
            CanDeleteParticipants = false,
            CanChangePublicity = false,
            CanChangeChatAvatar = false,
            CanChangeChatTitle = false,
            CanSetPermissions = false,
            CanDeleteConversation = false,
        },
        new()
        {
            Id = 2,
            AspNetUserId = "382A1E62-1A84-4722-8E53-F55ECBCBA3C3",
            ConversationId = 184623,
            CanWriteMessages = true,
            CanMuteParticipants = true,
            CanDeleteMessages = true,
            CanAddParticipants = true,
            CanDeleteParticipants = true,
            CanChangePublicity = true,
            CanChangeChatAvatar = true,
            CanChangeChatTitle = true,
            CanSetPermissions = true,
            CanDeleteConversation = true,
        },
        new()
        {
            Id = 3,
            AspNetUserId = "EC94280C-5E90-4D4C-9214-49799B4E5C92",
            ConversationId = 184623,
            CanWriteMessages = true,
            CanMuteParticipants = false,
            CanDeleteMessages = false,
            CanAddParticipants = false,
            CanDeleteParticipants = false,
            CanChangePublicity = false,
            CanChangeChatAvatar = false,
            CanChangeChatTitle = false,
            CanSetPermissions = false,
            CanDeleteConversation = false,
        },
        new()
        {
            Id = 852,
            AspNetUserId = "57453247-3B50-4034-90C3-47473C333E40",
            ConversationId = 51348,
            CanWriteMessages = true,
            CanMuteParticipants = false,
            CanDeleteMessages = false,
            CanAddParticipants = false,
            CanDeleteParticipants = false,
            CanChangePublicity = false,
            CanChangeChatAvatar = false,
            CanChangeChatTitle = false,
            CanSetPermissions = false,
            CanDeleteConversation = false,
        },
        new()
        {
            Id = 12384,
            AspNetUserId = "93F6E32F-FB4E-4A26-BA89-41437427C30D",
            ConversationId = 5588,
            CanWriteMessages = true,
            CanMuteParticipants = false,
            CanDeleteMessages = false,
            CanAddParticipants = false,
            CanDeleteParticipants = false,
            CanChangePublicity = false,
            CanChangeChatAvatar = false,
            CanChangeChatTitle = false,
            CanSetPermissions = false,
            CanDeleteConversation = false,
        },
        new()
        {
            Id = 1972,
            AspNetUserId = "00DA31F8-0619-4593-A89B-A39AA3FB88E7",
            ConversationId = 25482,
            CanWriteMessages = true,
            CanMuteParticipants = false,
            CanDeleteMessages = false,
            CanAddParticipants = false,
            CanDeleteParticipants = false,
            CanChangePublicity = false,
            CanChangeChatAvatar = false,
            CanChangeChatTitle = false,
            CanSetPermissions = false,
            CanDeleteConversation = false,
        },
    };

    private static List<Message> _fakeMessages = new()
    {
        new()
        {
            Id = 12648,
            ParticipationId = 3,
            MessageText = "First message",
            DateSent = new DateTime(638192609520000000),
        },
        new()
        {
            Id = 85678,
            ParticipationId = 2,
            MessageText = "Second message",
            DateSent = new DateTime(638194337520000000),
        },
        new()
        {
            Id = 1234565,
            ParticipationId = 3,
            MessageText = "Third message",
            DateSent = new DateTime(638196929520000000),
        },
        new()
        {
            Id = 15845,
            ParticipationId = 12384,
            MessageText = "Include AppUser test message",
            DateSent = DateTime.Today,
        },
    };

    private static List<ChatInfoView> _fakeChatInfoViews = new()
    {
        new()
        {
            ChatInfoId = 12345,
            ConversationId = 54321,
            Title = "Sample chat info view",
            ChatLink = "Sample_chat_info_view",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 47364,
            ConversationId = 54321,
            Title = "Sample chat0B513832CB504082945E842101768547 info view",
            ChatLink = "Sample_chat_info_view",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 75639,
            ConversationId = 951212,
            Title = "Sample info view",
            ChatLink = "Sample_chat0B513832CB504082945E842101768547_info_view",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 25486,
            ConversationId = 13845,
            Title = "Sample chat info view",
            ChatLink = "Sample_chat_8FC9455FF8834A70959376286A516A80_info_view",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 24157,
            ConversationId = 82468,
            Title = "Sample info view",
            ChatLink = "Sample_8FC9455FF8834A70959376286A516A80_chat_info_view",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 5615,
            ConversationId = 7612,
            Title = "Sample C77B4C0373B04B7F90D58626A4A57130 chat info view",
            ChatLink = "Sample_chat_info_view",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 1218,
            ConversationId = 13382,
            Title = "Sample info viC77B4C0373B04B7F90D58626A4A57130ew",
            ChatLink = "Sample_chat_info_view",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 99872,
            ConversationId = 45687,
            Title = "Sample 42291A2FD36F4492B4625E1EFC5D57EC chat info view",
            ChatLink = "Sample_ch31CB47FA19FA458684888912ECB95D4Fat_info_viC5088C799987455598303440E7727E0Cew",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 176523,
            ConversationId = 18578,
            Title = "Sample inCC5088C799987455598303440E7727E0Cfo vie42291A2FD36F4492B4625E1EFC5D57ECw",
            ChatLink = "Sample_ch31CB47FA19FA458684888912ECB95D4FCat_info_view",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 26134,
            ConversationId = 89456,
            Title = "!!!!!!!!!!!!!!!!!!!!!!!!!!!",
            ChatLink = "Sample_chat_info_view",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 12345565,
            ConversationId = 79183,
            Title = "ZZZZZZZZZZZZZZZZZZZZZZZZZZ",
            ChatLink = "Sample_chat_info_view",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 73145,
            ConversationId = 37541,
            Title = "Sample chat info view",
            ChatLink = "!!!!!!!!!!!!!!!!!!!!!!",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 24563,
            ConversationId = 6482,
            Title = "Sample chat info view",
            ChatLink = "ZZZZZZZZZZZZZZZZZZZZZZ",
            IsPrivate = false,
            ParticipationsCount = 325,
        },
        new()
        {
            ChatInfoId = 4682,
            ConversationId = 48251,
            Title = "Sample chat info view",
            ChatLink = "Sample_chat_info_view",
            IsPrivate = false,
            ParticipationsCount = int.MinValue,
        },
        new()
        {
            ChatInfoId = 26481,
            ConversationId = 73197,
            Title = "Sample chat info view",
            ChatLink = "Sample_chat_info_view",
            IsPrivate = false,
            ParticipationsCount = int.MaxValue,
        },
    };
}