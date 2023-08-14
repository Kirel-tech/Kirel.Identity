using Blazored.LocalStorage;
using Kirel.Identity.Client.Interfaces;

namespace Kirel.Identity.Client.Blazor.Services;

/// <summary>
/// JWT token storage service implemented with Blazored.LocalStorage
/// </summary>
public class KirelBlazoredClientTokenService : IClientTokenService
{
    private readonly ILocalStorageService _storage;

    /// <summary>
    /// Creates a JWT token storage service implemented with Blazored.LocalStorage
    /// </summary>
    /// <param name="storage"> </param>
    public KirelBlazoredClientTokenService(ILocalStorageService storage)
    {
        _storage = storage;
    }

    /// <inheritdoc />
    public async Task<string> GetAccessTokenAsync()
    {
        return await _storage.GetItemAsync<string>("AccessToken");
    }

    /// <inheritdoc />
    public async Task<string> GetRefreshTokenAsync()
    {
        return await _storage.GetItemAsync<string>("RefreshToken");
    }

    /// <inheritdoc />
    public async Task SetAccessTokenAsync(string token)
    {
        await _storage.SetItemAsync("AccessToken", token);
    }

    /// <inheritdoc />
    public async Task SetRefreshTokenAsync(string token)
    {
        await _storage.SetItemAsync("RefreshToken", token);
    }
}