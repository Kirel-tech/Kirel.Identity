using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Server.Core.Extensions;

/// <summary>
/// Extensions that adds fluent validation to DI
/// </summary>
public static class ValidatorsExtension
{
    /// <summary>
    /// Add fluent validators to DI
    /// </summary>
    /// <param name="services"> services collection </param>
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}