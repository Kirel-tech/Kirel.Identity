using System.Net.Http.Headers;
using Kirel.Identity.Client.Interfaces;
using Kirel.Identity.Client.Jwt.Helpers;

namespace Kirel.Identity.Client.Jwt.Handlers;

public class KirelJwtHttpClientAuthorizationHandler : DelegatingHandler
{
    private readonly IClientTokenService _tokenService;
    private readonly IClientAuthenticationService _authenticationService;
    public KirelJwtHttpClientAuthorizationHandler(IClientTokenService tokenService, IClientAuthenticationService authenticationService)
    {
        _tokenService = tokenService;
        _authenticationService = authenticationService;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var jwtAccessToken = await _tokenService.GetAccessTokenAsync();
        if (KirelJwtTokenHelper.TokenIsExpired(jwtAccessToken))
        {
            await _authenticationService.ExtendSessionAsync();
            jwtAccessToken = await _tokenService.GetAccessTokenAsync();
        }
        
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtAccessToken);
        return await base.SendAsync(request, cancellationToken);
    }
}