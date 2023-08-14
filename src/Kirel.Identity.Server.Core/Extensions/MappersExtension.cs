using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Server.Core.Extensions;

/// <summary>
/// Extension that adds AutoMapper to DI
/// </summary>
public static class MappersExtension
{
    /// <summary>
    /// Add AutoMapper to DI
    /// </summary>
    /// <param name="services"> services collection </param>
    public static void AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}