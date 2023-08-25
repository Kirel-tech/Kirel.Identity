using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Kirel.Identity.Server.API.Handlers;

/// <summary>
/// List of API keys for ApiKeyAuthenticationHandler
/// </summary>
public class ApiKeysList : List<string>
{
}


/// <summary>
/// Class that handles api key authentication
/// </summary>
public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ApiKeysList _apiKeys;

    /// <summary>
    /// Returns instance of api key authentication handler
    /// </summary>
    /// <param name="options">The monitor for the options instance.</param>
    /// <param name="logger">The <see cref="ILoggerFactory"/>.</param>
    /// <param name="encoder">The <see cref="System.Text.Encodings.Web.UrlEncoder"/>.</param>
    /// <param name="clock">The <see cref="ISystemClock"/>.</param>
    /// <param name="apiKeys">List of API keys</param>
    public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ApiKeysList apiKeys) : base(options, logger, encoder, clock)
    {
        _apiKeys = apiKeys;
    }

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.Authorization.ToString().StartsWith("APIKey"))
            return await Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        
        var apiKeyArr = Request.Headers.Authorization.ToString().Split(" ");
        if (apiKeyArr.Length < 2)
            return await Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        
        if (string.IsNullOrEmpty(apiKeyArr[1]))
        {
            return await Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }
        
        try
        {
            return ValidateApiKey(apiKeyArr[1]);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(AuthenticateResult.Fail(ex.Message));
        }
    }

    private AuthenticateResult ValidateApiKey(string apiKey)
    {
        if (!_apiKeys.Contains(apiKey))
        {
            return AuthenticateResult.Fail("Unauthorized"); 
        }
        var identity = GetClaimsIdentity();
        var principal = new System.Security.Principal.GenericPrincipal(identity, new []{"Microservice"});
        return AuthenticateResult.Success(new AuthenticationTicket(principal, "APIKey"));
    }
    
    private ClaimsIdentity GetClaimsIdentity()
    {
        var claims = new List<Claim>
        {
            new (ClaimsIdentity.DefaultNameClaimType, "Microservice"),
        };
        
        var claimsIdentity =
            new ClaimsIdentity(claims, "APIKey", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}