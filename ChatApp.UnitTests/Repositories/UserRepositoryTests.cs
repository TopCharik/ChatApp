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
    
    [OneTimeSetUp]
    public async Task SetUp()
    {
        var context = await FakeData.GetDefaultAppDbContext(); 
        _repository = new UserRepository(context);
    }

    [Test]
    public async Task GetUsersAsync_WithNoFilters_ReturnsPagedList()
    {
        var parameters = new AppUserParameters();

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
    }
        
    [Test]
    public async Task GetUsersAsync_WithSearchFilter_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            Search = "D05DE7939F274AB68853D0D9282FA284",
        };
        var expectedUsers = new List<AppUser>
        {
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
        };
        
        await VerifyUserListAsync(expectedUsers, parameters);
    }

    [Test]
    public async Task GetUsersAsync_WithUsernameFilter_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            UserName = "0A9BB93B62054848A048AB765E518A46",
        };
        var expectedUsers = new List<AppUser> 
        {
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
        };
        
        await VerifyUserListAsync(expectedUsers, parameters);
    }
    
    [Test]
    public async Task GetUsersAsync_WithFirstNameFilter_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            FirstName = "4602B55B7D184AD58F8ABDBFCFD12AC2",
        };
        var expectedUsers = new List<AppUser> 
        {
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
        };
        
        await VerifyUserListAsync(expectedUsers, parameters);
    }
    
    [Test]
    public async Task GetUsersAsync_WithLastNameFilter_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            LastName = "544A41B16E1F49A280A9CE557A0EBD35",
        };
        var expectedUsers = new List<AppUser> 
        {
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
        };
        
        await VerifyUserListAsync(expectedUsers, parameters);
    }
    
    [Test]
    public async Task GetUsersAsync_WithEmailFilter_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            Email = "8479664F638C4BB7AA390A055A3B918D",
        };
        var expectedUsers = new List<AppUser> 
        {
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
        };
        
        await VerifyUserListAsync(expectedUsers, parameters);
    }
    
    [Test]
    public async Task GetUsersAsync_WithPhoneNumberField_ReturnsPagedList()
    {
        var parameters = new AppUserParameters
        {
            PhoneNumber = "7339BA628BF341DC897F03E2ABDE8725",
        };
        var expectedUsers = new List<AppUser> 
        {
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
            UserName = "!!!!!!!!!!!!",
            NormalizedUserName = "!!!!!!!!!!!!",
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
            NormalizedUserName = "ZZZZZZ",
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
            FirstName = "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
            LastName = "Brown",
            UserName = "dbrown",
            NormalizedUserName = "DBROWN",
            Email = "dbrown@example.com",
            NormalizedEmail = "DBROWN@EXAMPLE.COM",
            PhoneNumber = "222-333-4444",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result.First().Id);
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
            FirstName = "ZZZZZZZZZZZZZZZZZZZZ",
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
            LastName = "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
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
            Email = "!!!!!!!!!!!!!!!!@example.com",
            NormalizedEmail = "!!!!!!!!!!!!!!!!@EXAMPLE.COM",
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
            PhoneNumber = "!!!!!!!!!!11-111-1111",
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
            PhoneNumber = "ZZZZZZZZZZ999-999-9999",
        };

        var result = await _repository.GetUsersAsync(parameters);

        Assert.AreEqual(FakeData.FakeUsers.Count, result.TotalCount);
        Assert.AreEqual(expectedFirstUser.Id, result[0].Id);
    }

    [Test]
    public async Task GetUserByUsernameAsync_WithExistingUsername_ReturnsUser()
    {
        const string username = "B73FB9C10A42455991B90EB4F0262699";
        var expectedResult = new AppUser
        {
            Id = "340E7DB9-1CBB-4B6D-84DD-DE95276E9AD7",
            FirstName = "John",
            LastName = "Doe",
            UserName = "B73FB9C10A42455991B90EB4F0262699",
            NormalizedUserName = "B73FB9C10A42455991B90EB4F0262699",
            Email = "johndoe@example.com",
            NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
            PhoneNumber = "+12-345-6789",
        };

        var appUser = await _repository.GetUserByUsernameAsync(username);

        Assert.IsNotNull(appUser);
        Assert.AreEqual(username, appUser.UserName);
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