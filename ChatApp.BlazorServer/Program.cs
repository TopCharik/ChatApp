using Blazored.LocalStorage;
using ChatApp.BlazorServer.ApiProviders;
using ChatApp.BlazorServer.Helpers;
using ChatApp.BlazorServer.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddHttpClient();
builder.Services.AddScoped<IJwtStorage, LocalStorageJwtStorage>();
builder.Services.AddScoped<IJwtHttpClientFactory, JwtHttpClientFactory>();
builder.Services.AddScoped<IAuthenticationApiProvider, AuthenticationApiProvider>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
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
