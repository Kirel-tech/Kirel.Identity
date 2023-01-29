using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Kirel.Identity.Client.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace Kirel.Identity.Client.Jwt.Providers;

public class KirelJwtTokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _anonymousUserAuthenticationState;
    private readonly IClientTokenService _tokenService;

    public KirelJwtTokenAuthenticationStateProvider(IClientTokenService tokenService)
    {
        _tokenService = tokenService;
        _anonymousUserAuthenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    /// <inheritdoc />
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = _anonymousUserAuthenticationState;
        var token = await _tokenService.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(token))
            state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(token), "JwtToken")));

        return state;
    }
    
    /// <summary>
    /// Notify authentication status changed
    /// </summary>
    public void NotifyStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
    {
        if (string.IsNullOrEmpty(jwt))
            return new List<Claim>();
        
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var claims = token.Claims;
        return claims;
    }
}