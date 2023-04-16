using ChatApp.API.Extensions;
using ChatApp.Core.Interfaces;
using ChatApp.DAL.App;
using ChatApp.DAL.AppContext;
using ChatApp.DAL.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


builder.Services.AddControllers();
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

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddSignInManager<SignInManager<IdentityUser>>();

builder.Services.AddJwtAuthentication(config);


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

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

app.Run();
