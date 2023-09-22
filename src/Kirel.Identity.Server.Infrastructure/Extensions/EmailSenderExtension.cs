using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Server.API.Configs;
using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Server.Infrastructure.Extensions;

/// <summary>
/// Extension for email sender
/// </summary>
public static class EmailSenderExtension
{
    /// <summary>
    /// Add email config and sender manager
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="cfg">Email config</param>
    /// <typeparam name="TEmailSenderManager">Email sender manager type</typeparam>
    public static void AddEmailSender<TEmailSenderManager>(this IServiceCollection services, EmailConfig cfg)
        where TEmailSenderManager : class, IKirelEmailSender
    {
        services.AddSingleton(cfg);
        services.AddScoped<IKirelEmailSender, TEmailSenderManager>();
    }
}