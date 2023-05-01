using Blazored.LocalStorage;
using ChatApp.BlazorServer.ApiProviders;
using ChatApp.BlazorServer.Helpers;
using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddHttpClient();
builder.Services.AddMudServices();
builder.Services.AddScoped<IJwtStorage, LocalStorageJwtStorage>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IJwtHttpClient, JwtHttpClient>();
builder.Services.AddScoped<IAuthenticationApiProvider, AuthenticationApiProvider>();
builder.Services.AddScoped<IUsersApiProvider, UsersApiProvider>();
builder.Services.AddScoped<IChatsApiProvider, ChatsApiProvider>();
builder.Services.AddScoped<IParticipationApiProvider, ParticipationApiProvider>();
builder.Services.AddScoped<IAvatarApiProvider, AvatarApiProvider>();
builder.Services.AddScoped<IMessagesApiProvider, MessagesApiProvider>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<IJwtHttpClient, JwtHttpClient>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddValidatorsFromAssemblyContaining<IValidation>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
