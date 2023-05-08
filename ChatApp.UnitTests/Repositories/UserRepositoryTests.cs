using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.Core.Helpers;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Repositories;
using NUnit.Framework;

namespace ChatApp.UnitTests.Repositories;

[TestFixture]
public class UserRepositoryTests
{
    private UserRepository _repository;
    private bool _firstSetUp = true;
    
    [SetUp]
    public async Task SetUp()
    {
        if (_firstSetUp)
        {
            var context = await FakeData.GetDefaultAppDbContext(); 
            _repository = new UserRepository(context);   
        }

        _firstSetUp = false;
    }

    [Test]
    public async Task GetUsersAsync_WithNoFilters_ReturnsPagedList()
    {
        var parameters = new AppUserParameters();

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(parameters.PageSize, result.Count);
    }
        
    [Test]
    public async Task GetUsersAsync_WithSearchFilter_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            Search = "doe",
        };
        var expectedUsers = new List<AppUser>
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
                PhoneNumber = "+12-345-6789",
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
                Id = "B4CB6F4D-76ED-4BEC-9C22-8A51D051A720",
                FirstName = "Mary",
                LastName = "Doe",
                UserName = "MaryD",
                NormalizedUserName = "MARYD",
                Email = "maryjohnson@example.com",
                NormalizedEmail = "MARYJOHNSON@EXAMPLE.COM",
                PhoneNumber = "555-987-6543",
            },
        };
        
        await VerifyUserListAsync(expectedUsers, parameters);
    }

    [Test]
    public async Task GetUsersAsync_WithUsernameFilter_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            UserName = "dOe",
        };
        var expectedUsers = new List<AppUser> 
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
                PhoneNumber = "+12-345-6789",
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
        
        await VerifyUserListAsync(expectedUsers, parameters);
    }
    
    [Test]
    public async Task GetUsersAsync_WithFirstNameFilter_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            FirstName = "oB",
        };
        var expectedUsers = new List<AppUser> 
        {
            new()
            {
                Id = "0D64737E-F602-44D8-9B0D-83D2E2C7FEBA",
                FirstName = "Robert",
                LastName = "Brown",
                UserName = "RobertB",
                NormalizedUserName = "ROBERTB",
                Email = "robertbrown@example.com",
                NormalizedEmail = "ROBERTBROWN@EXAMPLE.COM",
                PhoneNumber = "555-555-5555",
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
                PhoneNumber = "444-555-6666",
            },
        };
        
        await VerifyUserListAsync(expectedUsers, parameters);
    }
    
    [Test]
    public async Task GetUsersAsync_WithLastNameFilter_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            LastName = "dOe",
        };
        var expectedUsers = new List<AppUser> 
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
                PhoneNumber = "+12-345-6789",
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
                Id = "B4CB6F4D-76ED-4BEC-9C22-8A51D051A720",
                FirstName = "Mary",
                LastName = "Doe",
                UserName = "MaryD",
                NormalizedUserName = "MARYD",
                Email = "maryjohnson@example.com",
                NormalizedEmail = "MARYJOHNSON@EXAMPLE.COM",
                PhoneNumber = "555-987-6543",
            },
        };
        
        await VerifyUserListAsync(expectedUsers, parameters);
    }
    
    [Test]
    public async Task GetUsersAsync_WithEmailFilter_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            Email = "Doe@EXaMpLe.COm",
        };
        var expectedUsers = new List<AppUser> 
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
                PhoneNumber = "+12-345-6789",
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
        
        await VerifyUserListAsync(expectedUsers, parameters);
    }
    
    [Test]
    public async Task GetUsersAsync_WithPhoneNumberField_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            PhoneNumber = "45-6893",
        };

        var expectedUsers = new List<AppUser> 
        {
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
        };
        await VerifyUserListAsync(expectedUsers, parameters);
    }
    
    [Test]
    public async Task GetUsersAsync_SortByUserNameAscending_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            SortField = "UseRnAmE",
            SortDirection = SortDirection.Ascending,
        };
        var expectedFirstUser = new AppUser
        {
            Id = "5DE50785-60C3-4B5C-8EE8-6FFDBACEB671",
            FirstName = "David",
            LastName = "Brown",
            UserName = "AAAAAA",
            NormalizedUserName = "DBROWN",
            Email = "dbrown@example.com",
            NormalizedEmail = "DBROWN@EXAMPLE.COM",
            PhoneNumber = "222-333-4444",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }

    [Test]
    public async Task GetUsersAsync_SortByUserNameDescending_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            SortField = "UsERnAmE",
            SortDirection = SortDirection.Descending,
        };
        var expectedFirstUser = new AppUser
        {
            Id = "99078870-7BAB-44F6-97D8-340EC0EC2FFC",
            FirstName = "Emily",
            LastName = "Taylor",
            UserName = "ZZZZZZ",
            NormalizedUserName = "ETAYLOR",
            Email = "etaylor@example.com",
            NormalizedEmail = "ETAYLOR@EXAMPLE.COM",
            PhoneNumber = "555-666-7777",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }
    
    [Test]
    public async Task GetUsersAsync_SortByFirstNameAscending_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            SortField = "fIrSTnAmE",
            SortDirection = SortDirection.Ascending,
        };
        var expectedFirstUser = new AppUser
        {
            Id = "482B198F-1064-467F-B053-F9CEE91EF34E",
            FirstName = "AAAAAAA",
            LastName = "Brown",
            UserName = "dbrown",
            NormalizedUserName = "DBROWN",
            Email = "dbrown@example.com",
            NormalizedEmail = "DBROWN@EXAMPLE.COM",
            PhoneNumber = "222-333-4444",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }

    [Test]
    public async Task GetUsersAsync_SortByFirstNameDescending_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            SortField = "FirStNAMe",
            SortDirection = SortDirection.Descending,
        };
        var expectedFirstUser = new AppUser
        {
            Id = "24CBCD2A-D458-4502-8A3D-A47A90F20D2C",
            FirstName = "ZZZZZZZ",
            LastName = "Taylor",
            UserName = "etaylor",
            NormalizedUserName = "ETAYLOR",
            Email = "etaylor@example.com",
            NormalizedEmail = "ETAYLOR@EXAMPLE.COM",
            PhoneNumber = "555-666-7777",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }

    [Test]
    public async Task GetUsersAsync_SortByLastNameAscending_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            SortField = "laSTnAmE",
            SortDirection = SortDirection.Ascending,
        };
        var expectedFirstUser = new AppUser
        {
            Id = "FE74C855-04F5-44E5-9B04-BE69F0ED59E7",
            FirstName = "Carol",
            LastName = "AAAAA",
            UserName = "cdavis",
            NormalizedUserName = "CDAVIS",
            Email = "cdavis@example.com",
            NormalizedEmail = "CDAVIS@EXAMPLE.COM",
            PhoneNumber = "777-888-9999",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }

    [Test]
    public async Task GetUsersAsync_SortByLastNameDescending_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            SortField = "laSTnAmE",
            SortDirection = SortDirection.Descending,
        };
        var expectedFirstUser = new AppUser
        {
            Id = "A2B6FFA7-9320-466C-AC41-077F9463F570",
            FirstName = "David",
            LastName = "ZZZZZZ",
            UserName = "dbrown",
            NormalizedUserName = "DBROWN",
            Email = "dbrown@example.com",
            NormalizedEmail = "DBROWN@EXAMPLE.COM",
            PhoneNumber = "222-333-4444",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }

    [Test]
    public async Task GetUsersAsync_SortByEmailNameAscending_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            SortField = "eMAil",
            SortDirection = SortDirection.Ascending,
        };
        var expectedFirstUser = new AppUser
        {
            Id = "2CFDB80C-BBB3-4633-B47F-5F081CD7496C",
            FirstName = "David",
            LastName = "Brown",
            UserName = "dbrown",
            NormalizedUserName = "DBROWN",
            Email = "AAAAAA@example.com",
            NormalizedEmail = "AAAAAA@EXAMPLE.COM",
            PhoneNumber = "222-333-4444",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }

    [Test]
    public async Task GetUsersAsync_SortByEmailDescending_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            SortField = "EmaIL",
            SortDirection = SortDirection.Descending,
        };
        var expectedFirstUser = new AppUser
        {
            Id = "F8A3D859-28AB-40BE-BE19-9935A47F0EAE",
            FirstName = "Emily",
            LastName = "Taylor",
            UserName = "etaylor",
            NormalizedUserName = "ETAYLOR",
            Email = "ZZZZZ@example.com",
            NormalizedEmail = "ZZZZZ@EXAMPLE.COM",
            PhoneNumber = "555-666-7777",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }

    [Test]
    public async Task GetUsersAsync_SortByPhoneAscending_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            SortField = "PHonE",
            SortDirection = SortDirection.Ascending,
        };
        var expectedFirstUser = new AppUser
        {
            Id = "382A1E62-1A84-4722-8E53-F55ECBCBA3C3",
            FirstName = "David",
            LastName = "Brown",
            UserName = "dbrown",
            NormalizedUserName = "DBROWN",
            Email = "dbrown@example.com",
            NormalizedEmail = "DBROWN@EXAMPLE.COM",
            PhoneNumber = "111-111-1111",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }

    [Test]
    public async Task GetUsersAsync_SortByPhoneDescending_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            SortField = "pHONe",
            SortDirection = SortDirection.Descending,
        };
        var expectedFirstUser = new AppUser
        {
            Id = "38603CBC-C704-41C0-AA3E-D8B0E8A32F38",
            FirstName = "Emily",
            LastName = "Taylor",
            UserName = "etaylor",
            NormalizedUserName = "ETAYLOR",
            Email = "etaylor@example.com",
            NormalizedEmail = "ETAYLOR@EXAMPLE.COM",
            PhoneNumber = "999-999-9999",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }

    [Test]
    public async Task GetUserByUsernameAsync_WithExistingUsername_ReturnsUser()
    {
        const string username = "JohnDoe";
        var expectedResult = new AppUser
        {
            Id = "71487432-92E3-49F9-8ECB-31FFD92FD581",
            FirstName = "John",
            LastName = "Doe",
            UserName = "JohnDoe",
            NormalizedUserName = "JOHNDOE",
            Email = "johndoe@example.com",
            NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
            PhoneNumber = "+12-345-6789",
        };

        var appUser = await _repository.GetUserByUsernameAsync(username);

        Assert.IsNotNull(appUser);
        Assert.AreEqual("JohnDoe", appUser.UserName);
        Assert.AreEqual(expectedResult.Id, appUser.Id);
    }

    [Test]
    public async Task GetUserByUsernameAsync_WithPagingParameters_ReturnsNull()
    {
        var parameters = new AppUserParameters
        {
            PageSize = 5,
            Page = 2,
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(parameters.PageSize, result.PageSize);
        Assert.AreEqual(parameters.Page, result.CurrentPage);
    }
    
    [Test]
    public async Task GetUserByUsernameAsync_WithPageSizeAboveMaximumParameters_ReturnsNull()
    {
        var parameters = new AppUserParameters
        {
            PageSize = 100,
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(50, result.PageSize);
    }

    [Test]
    public async Task GetUserByUsernameAsync_WithNonExistingUsername_ReturnsNull()
    {
        var username = "nonexistinguser";

        var result = await _repository.GetUserByUsernameAsync(username);

        Assert.IsNull(result);
    }

    private async Task VerifyUserListAsync(List<AppUser> expectedUsers, AppUserParameters parameters)
    {
        var expectedResult = PagedList<AppUser>.ToPagedList(expectedUsers, parameters.Page, parameters.PageSize);

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(expectedResult.TotalCount, result.TotalCount);
        Assert.AreEqual(expectedResult.Count, result.Count);
        Assert.True(expectedResult.All(expectedUser => result.FirstOrDefault(x => x.Id == expectedUser.Id) != null));
    }
}