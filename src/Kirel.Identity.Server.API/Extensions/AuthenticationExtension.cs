using System.Text;
using Kirel.Identity.Server.Jwt.Shared;
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
    public static void AddAuthenticationConfiguration(this IServiceCollection services, JwtAuthenticationConfig config)
    {
        services.AddAuthentication(option =>
            {
                // Fixing 404 error when adding an attribute Authorize to controller
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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