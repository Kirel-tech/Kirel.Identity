using System.Text;
using Kirel.Identity.Server.API.Handlers;
using Kirel.Identity.Server.Jwt.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Kirel.Identity.Server.API.Extensions;

/// <summary>
/// Authentication configuration extension
/// </summary>
public static class AuthenticationExtension
{
    /// <summary>
    /// Add authentication configuration to DI
    /// </summary>
    /// <param name="services"> services collection </param>
    /// <param name="config"> JWT Token generation config </param>
    /// <param name="apiKeys">API keys list</param>
    public static void AddAuthenticationConfiguration(this IServiceCollection services, JwtAuthenticationConfig config, ApiKeysList apiKeys)
    {
        services.AddSingleton(apiKeys);
        services.AddAuthentication(option =>
            {
                // Fixing 404 error when adding an attribute Authorize to controller
                option.DefaultAuthenticateScheme = "Smart";
                option.DefaultChallengeScheme = "Smart";
            })
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("APIKey", null)
            .AddPolicyScheme("Smart", "Bearer JWT or API key authorization", options =>
            {
                options.ForwardDefaultSelector = context => context.Request.Headers["Authorization"]
                    .ToString()?.StartsWith("Bearer ") == true ? JwtBearerDefaults.AuthenticationScheme : "APIKey";
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config.Issuer,
                    ValidateAudience = true,
                    ValidAudience = config.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Key)),
                    ValidateIssuerSigningKey = true
                };
            });
    }
}