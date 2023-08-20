using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kirel.Identity.Server.API.Filters;

/// <inheritdoc />
public class DisabledControllerTypes : List<Type>
{
}

/// <summary>
/// Custom attribute for enabling or disabling Swagger documentation based on a specified setting.
/// </summary>
public class EnabledControllerDocumentFilter : IDocumentFilter
{
    private readonly DisabledControllerTypes _disabledControllerTypes;
    /// <summary>
    /// Initializes a new instance of the <see cref="EnabledControllerDocumentFilter"/> class.
    /// </summary>
    /// <param name="disabledControllerTypes">The list of disabled controller types.</param>
    public EnabledControllerDocumentFilter(DisabledControllerTypes disabledControllerTypes)
    {
        _disabledControllerTypes = disabledControllerTypes;
    }
    
    /// <summary>
    /// Removes paths associated with a specific controller from the OpenAPI document.
    /// </summary>
    /// <param name="swaggerDoc">The OpenAPI document to modify.</param>
    /// <param name="controllerType">The type of the controller whose paths should be removed.</param>
    private void RemoveControllerPaths(OpenApiDocument swaggerDoc, Type controllerType)
    {
        var controllerRouteAttribute = controllerType.GetCustomAttribute<RouteAttribute>();

        if (controllerRouteAttribute != null)
        {
            var controllerRoute = controllerRouteAttribute.Template;
            var pathsToRemove = swaggerDoc.Paths.Keys
                .Where(path => path.StartsWith($"/{controllerRoute}", StringComparison.OrdinalIgnoreCase))
                .ToList();
            foreach (var pathToRemove in pathsToRemove)
            {
                swaggerDoc.Paths.Remove(pathToRemove);
            }
        }
    }

    /// <summary>
    /// Applies the filter to the Swagger/OpenAPI document. If a controller is disabled, removes its paths from the document.
    /// </summary>
    /// <param name="swaggerDoc">The Swagger document.</param>
    /// <param name="context">The filter context.</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var controllerType in _disabledControllerTypes)
            RemoveControllerPaths(swaggerDoc, controllerType);
    }
}