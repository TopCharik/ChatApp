using ChatApp.BlazorServer;
using ChatApp.BlazorServer.ApiProviders;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddHttpClient();
builder.Services.AddTransient<IJwtHttpClientFactory, LocalStorageJwtHttpClientFactory>();
builder.Services.AddTransient<IAuthenticationApiProvider, AuthenticationApiProvider>();


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
