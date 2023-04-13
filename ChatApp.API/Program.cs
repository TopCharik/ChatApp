using System.Text;
using ChatApp.API;
using ChatApp.API.Extensions;
using ChatApp.Core.Entities.FooAggregate;
using ChatApp.Core.Entities.Identity;
using ChatApp.Core.Interfaces;
using ChatApp.DAL;
using ChatApp.DAL.AppContext;
using ChatApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

builder.Services.AddDbContext<AppDbContext>(opt => opt
    .UseSqlServer(config.GetConnectionString("LocalDbConnection"))
);



builder.Services.AddJwtAuthentication(config);
builder.Services.AddSingleton<IJwtConfiguration, JwtConfiguration>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFooService, FooService>();

var app = builder.Build();
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
