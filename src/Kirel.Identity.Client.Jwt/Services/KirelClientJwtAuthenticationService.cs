using System.Net.Http.Headers;
using System.Net.Http.Json;
using Kirel.Identity.Client.Interfaces;
using Kirel.Identity.Client.Jwt.Helpers;
using Kirel.Identity.Client.Jwt.Options;
using Kirel.Identity.Client.Jwt.Providers;
using Kirel.Identity.Jwt.DTOs;
using Microsoft.Extensions.Options;

namespace Kirel.Identity.Client.Jwt.Services;

/// <summary>
/// Client authentication service with JWT implementation.
/// </summary>
public class KirelClientJwtAuthenticationService : IClientAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly KirelJwtTokenAuthenticationStateProvider _stateProvider;
    private readonly IClientTokenService _tokenService;
    private readonly string _url;

    /// <summary>
    /// Creates new instance of Client authentication service with JWT implementation.
    /// </summary>
    /// <param name="tokenService"> Client token service that stores tokens </param>
    /// <param name="stateProvider"> Implementation of default AuthenticationStateProvider that works with JWT tokens. </param>
    /// <param name="httpClientFactory"> Default microsoft http client factory </param>
    /// <param name="options"> Client authentication options </param>
    public KirelClientJwtAuthenticationService(
        IClientTokenService tokenService,
        KirelJwtTokenAuthenticationStateProvider stateProvider,
        IHttpClientFactory httpClientFactory,
        IOptions<KirelClientJwtAuthenticationOptions> options)
    {
        _tokenService = tokenService;
        _stateProvider = stateProvider;
        _httpClient = httpClientFactory.CreateClient();
        _url = options.Value.RelativeUrl;
        if (!string.IsNullOrEmpty(options.Value.BaseUrl))
            _url = options.Value.BaseUrl + _url;
    }

    /// <inheritdoc />
    public async Task LoginByPasswordAsync(string type, string login, string password)
    {
        var tokenDto = await _httpClient.GetFromJsonAsync<JwtTokenDto>(
            $"{_url}?type={type}&login={login}&password={password}");
        //TODO: Add logging here
        if (tokenDto == null)
            return;
        await UpdateTokens(tokenDto.AccessToken, tokenDto.RefreshToken)
            .ContinueWith(_ => _stateProvider.NotifyStateChanged());
    }

    /// <inheritdoc />
    public async Task<DateTimeOffset> GetSessionExpirationTimeAsync()
    {
        var refreshToken = await _tokenService.GetRefreshTokenAsync();
        return string.IsNullOrEmpty(refreshToken)
            ? DateTimeOffset.MinValue
            : KirelJwtTokenHelper.GetExpirationTime(refreshToken);
    }

    /// <inheritdoc />
    public async Task ExtendSessionAsync()
    {
        var refreshToken = await _tokenService.GetRefreshTokenAsync();
        if (string.IsNullOrEmpty(refreshToken) || KirelJwtTokenHelper.TokenIsExpired(refreshToken)) return;

        var request = new HttpRequestMessage(HttpMethod.Put, $"{_url}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshToken);
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var tokenDto = await response.Content.ReadFromJsonAsync<JwtTokenDto>();
            if (tokenDto != null)
                await UpdateTokens(tokenDto.AccessToken, tokenDto.RefreshToken)
                    .ContinueWith(_ => _stateProvider.NotifyStateChanged());
        }
    }

    /// <inheritdoc />
    public async Task<bool> SessionIsActiveAsync()
    {
        var refreshToken = await _tokenService.GetRefreshTokenAsync();
        return !string.IsNullOrEmpty(refreshToken) && !KirelJwtTokenHelper.TokenIsExpired(refreshToken);
    }

    /// <inheritdoc />
    public async Task Logout()
    {
        await UpdateTokens("", "")
            .ContinueWith(_ => _stateProvider.NotifyStateChanged());
    }

    private async Task UpdateTokens(string access, string refresh)
    {
        await _tokenService.SetAccessTokenAsync(access);
        await _tokenService.SetRefreshTokenAsync(refresh);
    }
}