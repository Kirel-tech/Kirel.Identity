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
    /// <param name="cfg">Sms sender config</param>
    /// <typeparam name="TSmsSenderManager">Sms sender manager type which is implementation of ISmsSender </typeparam>
    public static void AddSmsAuthentication<TSmsSenderManager>(this IServiceCollection services, MainSmsSenderConfig cfg)
    where TSmsSenderManager : class, ISmsSender
    {
        services.AddSingleton(cfg);
        services.AddScoped<ISmsSender, TSmsSenderManager>();
        services.AddScoped<SmsAuthenticationService>();
        services.AddScoped<SmsConfirmationService>();
    }
}