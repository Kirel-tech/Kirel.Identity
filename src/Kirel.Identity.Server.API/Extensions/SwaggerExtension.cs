using Kirel.Identity.Server.API.Filters;
using Kirel.Identity.Server.Swagger.Shared;
using Microsoft.OpenApi.Models;

namespace Kirel.Identity.Server.API.Extensions;

/// <summary>
/// Extension that adds swagger documentation support
/// </summary>
public static class SwaggerExtension
{
    /// <summary>
    /// Add swagger documentation stuff to DI
    /// </summary>
    /// <param name="services"> </param>
    /// <param name="disabledControllers"> </param>
    public static void AddSwagger(this IServiceCollection services, List<Type> disabledControllers)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            //Add swagger xml docs
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Server API", Version = "v1" });
            c.DocumentFilter<EnabledControllerDocumentFilter>(disabledControllers);
            //Set the comments path for the swagger json and ui.
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly)
                .ToList();
            xmlFiles.ForEach(xmlFile => c.IncludeXmlComments(xmlFile));
            // Add JWT token authorization support
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
            });
            c.AddSecurityDefinition("APIKey", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "APIKey",
                In = ParameterLocation.Header,
                Description =
                    "API key authorization header using the APIKey scheme. \r\n\r\n Enter 'APIKey' [space] and then your API key in the text input below.\r\n\r\nExample: \"APIKey 12345abcdef\""
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "APIKey"
                        }
                    },
                    new string[] { }
                }
            });
            c.OperationFilter<GeneralExceptionOperationFilter>();
        });
    }
}
