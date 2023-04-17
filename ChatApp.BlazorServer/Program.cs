using Blazored.LocalStorage;
using ChatApp.BlazorServer;
using ChatApp.BlazorServer.ApiProviders;
using ChatApp.BlazorServer.Authentication;
using ChatApp.BlazorServer.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddHttpClient();
builder.Services.AddScoped<IJwtHttpClientFactory, LocalStorageJwtHttpClientFactory>();
builder.Services.AddScoped<IAuthenticationApiProvider, AuthenticationApiProvider>();
builder.Services.AddScoped<IJwtPersistService, LocalStorageJwtPersistService>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();


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
