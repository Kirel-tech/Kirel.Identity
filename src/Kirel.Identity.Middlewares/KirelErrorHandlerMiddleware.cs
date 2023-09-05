using System.Text.Json;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Middlewares;

/// <summary>
/// Middleware for global error handling
/// </summary>
public class KirelErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor of ErrorHandlerMiddleware
    /// </summary>
    /// <param name="next"> RequestDelegate </param>
    public KirelErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private async Task SetProblemDetails(HttpContext context, int code, string title, string details)
    {
        context.Response.StatusCode = code;
        context.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails
        {
            Title = title,
            Detail = details,
            Status = context.Response.StatusCode,
            Type = "about:blank"
        };
        var jsonToken = JsonSerializer.Serialize(problem);
        await context.Response.WriteAsync(jsonToken);
    }

    /// <summary>
    /// Middleware call point
    /// </summary>
    /// <param name="context"> HttpContext </param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KirelIdentityStoreException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status500InternalServerError,
            context.Response.StatusCode.ToString(), ex.Message);
        }
        catch (KirelValidationException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status400BadRequest,
            context.Response.StatusCode.ToString(), ex.Message);
        }
        catch (KirelAlreadyExistException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status409Conflict,
            context.Response.StatusCode.ToString(), ex.Message);
        }
        catch (KirelAuthenticationException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status401Unauthorized,
            context.Response.StatusCode.ToString(), ex.Message);
        }
        catch (KirelUnauthorizedException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status403Forbidden,
            context.Response.StatusCode.ToString(), ex.Message);
        }
        catch (KirelNotFoundException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status404NotFound,
            context.Response.StatusCode.ToString(), ex.Message);
        }
    }
}