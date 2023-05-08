using System.Collections.Concurrent;
using ChatApp.BlazorServer.Services.Authentication;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatApp.BlazorServer.StateContainers;

public class HubConnectionProvider : IDisposable
{
    private readonly IJwtProvider _jwtProvider;
    private readonly string? _apiUrl;
    private ConcurrentDictionary<string, HubConnection> _hubs = new();

    public HubConnectionProvider(IJwtProvider jwtProvider, IConfiguration config)
    {
        _jwtProvider = jwtProvider;
        _apiUrl = config["ApiUrl"] ?? throw new ArgumentException("Api url not found in configuration");
    }

    public async Task<HubConnection> GetHubConnection(string hubUrl)
    {
        var result = _hubs.TryGetValue(hubUrl, out var hub);
        if (!result)
        {
            hub = new HubConnectionBuilder().WithUrl($"{_apiUrl}/hub/{hubUrl}", connectionOptions =>
            {
                connectionOptions.AccessTokenProvider = async () => await _jwtProvider.GetTokenAsync();
            }).Build();
            await hub.StartAsync();
            _hubs.TryAdd(hubUrl, hub);
        }

        return hub;
    }

    public async Task DisconnectFromHubsAsync()
    {
        foreach (var hub in _hubs)
        {
            await hub.Value.DisposeAsync();
        }
        _hubs.Clear();
    }

    public void Dispose()
    {
        DisconnectFromHubsAsync();
    }
}