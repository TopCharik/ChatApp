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
        List<Message>? messages = null
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
        await context.SaveChangesAsync();
        return context;
    }
    
    private static List<AppUser> _fakeUsers = new()
    {
            new()
            {
                Id = "71487432-92E3-49F9-8ECB-31FFD92FD581",
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
                Id = "2D829992-8FB0-4800-98A2-0E4E429C6708",
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
                LastName = "AAAAA",
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
                FirstName = "AAAAAAA",
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
                FirstName = "ZZZZZZZ",
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
                UserName = "AAAAAA",
                NormalizedUserName = "DBROWN",
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
                NormalizedUserName = "ETAYLOR",
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
                Email = "AAAAAA@example.com",
                NormalizedEmail = "AAAAAA@EXAMPLE.COM",
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
                PhoneNumber = "+11-111-1111",
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
                PhoneNumber = "999-999-9999",
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
        };

    private static List<Avatar> _fakeAvatars = new()
    {
        new()
        {
            UserId = "436E26E8-45B8-4971-A81B-663A40A93893",
            DateSet = DateTime.Now,
            ImagePayload = "qwerty",
        },
        new()
        {
            UserId = "2D829992-8FB0-4800-98A2-0E4E429C6708",
            DateSet = DateTime.Now,
            ImagePayload = "qwerty",
        },
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
            Id = 3,
            ChatInfoId = 3,  
        },
        new()
        {
            Id = 1234565,
            ChatInfoId = 4,  
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
            Id = 3,
            Title = "Third Chat",
            ChatLink = "Third_Chat",
            IsPrivate = true,
        },
        new()
        {
            Id = 4,
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
    };

    private static List<Participation> _fakeParticipations = new()
    {
        new()
        {
            Id = 1,
            AspNetUserId = "661ABAAB-DA8F-46B9-B796-52A258C591BF",
            ConversationId = 1234565,
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
            AspNetUserId = "2D829992-8FB0-4800-98A2-0E4E429C6708",
            ConversationId = 1234565,
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
            AspNetUserId = "2D829992-8FB0-4800-98A2-0E4E429C6708",
            ConversationId = 1234565,
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
            ParticipationId = 2, 
            MessageText = "First message",
            DateSent = new DateTime(638191744920000000),
        },
        new()
        {
            Id = 85678,
            ParticipationId = 2,
            MessageText = "Second message",
            DateSent = new DateTime(638191746120000000),
        },
        new()
        {
            Id = 1234565,
            ParticipationId = 3,
            MessageText = "Third message",
            DateSent = new DateTime(638191746200000000),
        },
    };
}