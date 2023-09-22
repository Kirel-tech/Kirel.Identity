using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Server.Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Server.Infrastructure.Extensions;

/// <summary>
/// Extension for add sms sender
/// </summary>
public static class SmsSenderExtension
{
    /// <summary>
    /// Adds sms sender cfg and manager
    /// </summary>
    /// <param name="services">Sms sender manager</param>
    /// <param name="cfg">Sms config</param>
    /// <typeparam name="TSmsSenderManager">IKirelSmsSender type</typeparam>
    public static void AddSmsSender<TSmsSenderManager>(this IServiceCollection services, MainSmsSenderConfig cfg)
        where TSmsSenderManager : class, IKirelSmsSender
    {
        services.AddSingleton(cfg);
        services.AddScoped<IKirelSmsSender, TSmsSenderManager>();
    }
}