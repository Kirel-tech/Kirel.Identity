using Kirel.Identity.Core.Models;

using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Jwt.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Server.Core.Extensions;

/// <summary>
/// Extensions that adds identity services
/// </summary>
public static class ServicesExtension
{
    /// <summary>
    /// Add identity services
    /// </summary>
    /// <param name="services"> services collection </param>
    /// <param name="jwtConfig"> JWT Token generation config </param>
    public static void AddServices(this IServiceCollection services, JwtAuthenticationConfig jwtConfig)
    {
        services.AddScoped<AuthenticationService>();
        services.AddScoped<AuthorizedUserService>();
        services.AddScoped<UserService>();
        services.AddScoped<RoleService>();
        services.AddScoped<RegistrationService>();
        services.AddScoped<SmsAuthenticationService>();
        services.AddScoped<SmsConfirmationService>();
        services.AddScoped<UserInviteService>();

        // configure JWT Tokens issue
        var kirelAuthOptions = new KirelAuthOptions
        {
            Audience = jwtConfig.Audience,
            Issuer = jwtConfig.Issuer,
            Key = jwtConfig.Key,
            AccessLifetime = jwtConfig.AccessLifetime,
            RefreshLifetime = jwtConfig.RefreshLifetime
        };
        services.AddSingleton(kirelAuthOptions);
        services.AddScoped<JwtTokenService>();
    }
}