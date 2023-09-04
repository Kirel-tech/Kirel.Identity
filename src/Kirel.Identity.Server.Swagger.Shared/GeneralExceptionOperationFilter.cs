using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kirel.Identity.Server.Swagger.Shared;

public class GeneralExceptionOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Add schema inside if to exclude multiple adding situation
        var schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository);
        operation.Responses.TryAdd("400", new OpenApiResponse
        {
            Description = "Bad request", 
            Content = new Dictionary<String, OpenApiMediaType>()
            {
                { 
                    "application/problem+json", new OpenApiMediaType()
                    {
                        Schema = schema,
                    }
                }
            }
        });
        operation.Responses.TryAdd("404", new OpenApiResponse
        {
            Description = "Service in maintenance mode",
            Content = new Dictionary<String, OpenApiMediaType>()
            {
                { 
                    "application/problem+json", new OpenApiMediaType()
                    {
                        Schema = schema,
                    }
                }
            }
        });
        operation.Responses.TryAdd("409", new OpenApiResponse
        {
            Description = "Conflict",
            Content = new Dictionary<String, OpenApiMediaType>()
            {
                { 
                    "application/problem+json", new OpenApiMediaType()
                    {
                        Schema = schema,
                    }
                }
            }
        });
        operation.Responses.TryAdd("500", new OpenApiResponse
        {
            Description = "Internal server error",
            Content = new Dictionary<String, OpenApiMediaType>()
            {
                { 
                    "application/problem+json", new OpenApiMediaType()
                    {
                        Schema = schema,
                    }
                }
            }
        });
        operation.Responses.TryAdd("503", new OpenApiResponse
        {
            Description = "Service in maintenance mode",
            Content = new Dictionary<String, OpenApiMediaType>()
            {
                { 
                    "application/problem+json", new OpenApiMediaType()
                    {
                        Schema = schema,
                    }
                }
            }
        });
        //Example where we filter on specific HttpMethod and define the return model
        var authorizeAttribute = context.MethodInfo.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .FirstOrDefault();
        if (authorizeAttribute != null)
        {
            operation.Responses.TryAdd("401", new OpenApiResponse
            {
                Description = "Unauthorized",
                Content = new Dictionary<String, OpenApiMediaType>()
                {
                    { 
                        "application/problem+json", new OpenApiMediaType()
                        {
                            Schema = schema,
                        }
                    }
                }
            });
            if (authorizeAttribute.Roles?.Split(",", StringSplitOptions.TrimEntries).Length > 1 ||
                authorizeAttribute.AuthenticationSchemes?.Split(",", StringSplitOptions.TrimEntries).Length > 1)
            {
                operation.Responses.TryAdd("403", new OpenApiResponse
                {
                    Description = "Forbidden",
                    Content = new Dictionary<String, OpenApiMediaType>()
                    {
                        { 
                            "application/problem+json", new OpenApiMediaType()
                            {
                                Schema = schema,
                            }
                        }
                    }
                });
            }
        }
    }
}