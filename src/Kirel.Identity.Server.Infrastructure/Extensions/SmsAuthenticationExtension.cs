using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Infrastructure.Managers;
using Kirel.Identity.Server.Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Server.Infrastructure.Extensions;

/// <summary>
/// Extension for add sms authentication
/// </summary>
public static class SmsAuthenticationExtension
{
    /// <summary>
    /// Add sms config, manager and services
    /// </summary>
    /// <param name="services">Services collection</param>
    public static void AddSmsAuthentication(this IServiceCollection services)
    {
        services.AddScoped<SmsAuthenticationService>();
        services.AddScoped<SmsConfirmationService>();
    }
}