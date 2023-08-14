using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kirel.Identity.Server.Swagger.Shared;

public class GeneralExceptionOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
        operation.Responses.Add("400", new OpenApiResponse { Description = "Bad request" });
        operation.Responses.Add("500", new OpenApiResponse { Description = "Internal server error" });
        operation.Responses.Add("503", new OpenApiResponse { Description = "Service in maintenance mode" });
        //Example where we filter on specific HttpMethod and define the return model
        var authorizeAttribute = context.MethodInfo.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .FirstOrDefault();
        if (authorizeAttribute != null)
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
    }
}