using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kirel.Identity.Server.API.Filters;

/// <summary>
/// Swagger operations filter for add security definitions for endpoints
/// </summary>
public class AuthRequirementOperationFilter : IOperationFilter
{
    private const string DefaultSecurityScheme = "Bearer";
    /// <summary>
    /// Add security definitions for endpoints
    /// </summary>
    /// <param name="operation"> OpenAPI operation </param>
    /// <param name="context"> Operation filter context </param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authorizedAttribute = context.MethodInfo.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .FirstOrDefault();
        if (authorizedAttribute == null)
            return;
        var schemes = authorizedAttribute.AuthenticationSchemes?.Split(",", StringSplitOptions.TrimEntries);
        if (schemes == null)
        {
            operation.Security.Add(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme, Id = DefaultSecurityScheme,
                        }
                    },
                    new string[]
                    {
                    }
                }
            });
            return;
        }

        var securityRequirement = new OpenApiSecurityRequirement();
        foreach (var scheme in schemes)
        {
            securityRequirement.Add(new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = scheme,
                }
            }, new string[] { });
        }
        operation.Security.Add(securityRequirement);
    }
}