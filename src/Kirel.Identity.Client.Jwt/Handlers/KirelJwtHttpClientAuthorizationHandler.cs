using System.Net.Http.Headers;
using Kirel.Identity.Client.Interfaces;
using Kirel.Identity.Client.Jwt.Helpers;
using Microsoft.AspNetCore.Components;

namespace Kirel.Identity.Client.Jwt.Handlers;

public class KirelJwtHttpClientAuthorizationHandler : DelegatingHandler
{
    private readonly IClientTokenService _tokenService;
    private readonly IClientAuthenticationService _authenticationService;
    private readonly NavigationManager _navigationManager;
    public KirelJwtHttpClientAuthorizationHandler(IClientTokenService tokenService, IClientAuthenticationService authenticationService,
        NavigationManager navigationManager)
    {
        _tokenService = tokenService;
        _authenticationService = authenticationService;
        _navigationManager = navigationManager;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var jwtAccessToken = await _tokenService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(jwtAccessToken) || KirelJwtTokenHelper.TokenIsExpired(jwtAccessToken))
        {
            await _authenticationService.ExtendSessionAsync()
                .ContinueWith(async _ =>
                {
                    if (!await _authenticationService.SessionIsActiveAsync())
                    {
                        _navigationManager.NavigateTo("/session/expired");
                    }
                }, cancellationToken)
                .ContinueWith(async _ => jwtAccessToken = await _tokenService.GetAccessTokenAsync(), cancellationToken);
        }

        if (!string.IsNullOrEmpty(jwtAccessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtAccessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}