using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Options;
using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Jwt.Shared;
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
    /// <param name="emailSettings"> emailSettings options </param>
    public static void AddServices(this IServiceCollection services, JwtAuthenticationConfig jwtConfig,
        EmailSettings emailSettings)
    {
        services.AddScoped<AuthenticationService>();
        services.AddScoped<AuthorizedUserService>();
        services.AddScoped<UserService>();
        services.AddScoped<RoleService>();
        services.AddScoped<RegistrationService>();
        services.AddScoped<IMailSender, SmtpMailSender>();
        services.AddScoped<EmailAuthenticationService>();


        var emailsettings = new EmailSettings
        {
            SmtpServer = emailSettings.SmtpServer,
            SmtpPort = emailSettings.SmtpPort,
            SmtpUsername = emailSettings.SmtpUsername,
            SmtpPassword = emailSettings.SmtpPassword,
            SmtpEmail = emailSettings.SmtpEmail
        };
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
        services.AddSingleton(emailsettings);
        services.AddScoped<JwtTokenService>();
        services.AddScoped<EmailConfirmationService>();
    }
}