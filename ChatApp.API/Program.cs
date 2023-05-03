using ChatApp.API.Extensions;
using ChatApp.API.Helpers;
using ChatApp.API.Hubs;
using ChatApp.API.Middlewares;
using ChatApp.BLL;
using ChatApp.DAL.App;
using ChatApp.DAL.App.AppContext;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using ChatApp.DAL.Identity;
using ChatApp.DTO.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", p =>
    {
        p.WithOrigins(config["BlazorClientUrl"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddDbContext<AppDbContext>(opt => opt
    .UseSqlServer(config.GetConnectionString("LocalDbConnection"))
);

builder.Services.AddDbContext<IdentityDbContext>(opt => opt
    .UseSqlServer(config.GetConnectionString("LocalDbConnection"))
);

builder.Services.AddIdentityCore<ExtendedIdentityUser>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddSignInManager<SignInManager<ExtendedIdentityUser>>();

builder.Services.AddJwtAuthentication(config);



builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IConversationsRepository, ConversationsRepository>();
builder.Services.AddScoped<IParticipationRepository, ParticipationRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IAvatarRepository, AvatarRepository>();
builder.Services.AddScoped<IAvatarService, AvatarService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddValidatorsFromAssemblyContaining<IValidation>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("BlazorClient");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ConversationsHub>("/hub/conversations");
app.MapHub<CallsHub>("/hub/calls");
app.MapHub<UsersHub>("/hub/users");

app.Run();
