using ChatApp.BLL;
using ChatApp.DAL.App;
using ChatApp.DAL.App.AppContext;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using ChatApp.DAL.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.IntegrationTests;

public abstract class BaseServiceTestFixture
{
    private const string TESTDB_CONNECTION_STRING = "Data Source=localhost,1433;Database=IntegrationTestChatAppDB;User Id=SA;Password=i23456789!;TrustServerCertificate=true";
    
    protected IServiceProvider _serviceProvider;
    protected DbContext _dbContext;
    protected UserManager<ExtendedIdentityUser> _userManager;


    protected BaseServiceTestFixture()
    {
        var serviceCollection = new ServiceCollection()
            .AddDbContext<AppDbContext>(opt => opt.UseSqlServer(TESTDB_CONNECTION_STRING))
            .AddDbContext<IdentityDbContext>(opt => opt.UseSqlServer(TESTDB_CONNECTION_STRING))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IConversationsRepository, ConversationsRepository>()
            .AddScoped<IParticipationRepository, ParticipationRepository>()
            .AddScoped<IMessageRepository, MessageRepository>()
            .AddScoped<IAvatarRepository, AvatarRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IMessageService, MessageService>();
        serviceCollection.AddIdentityCore<ExtendedIdentityUser>()
            .AddEntityFrameworkStores<IdentityDbContext>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
        
        _dbContext = _serviceProvider.GetService<AppDbContext>() 
                          ?? throw new InvalidOperationException("IMessageService is not registered in BaseServiceTestFixture");
        _userManager = _serviceProvider.GetService<UserManager<ExtendedIdentityUser>>()
                       ?? throw new InvalidOperationException("UserManager is not registered in BaseServiceTestFixture");
    }
}