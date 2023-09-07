using System.Text.Json;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

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

    private async Task SetProblemDetails(HttpContext context, int code, string details)
    {
        context.Response.StatusCode = code;
        context.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails
        {
            Title = ReasonPhrases.GetReasonPhrase(code),
            Detail = details,
            Status = code,
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
            await SetProblemDetails(context, StatusCodes.Status500InternalServerError, ex.Message);
        }
        catch (KirelValidationException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (KirelAlreadyExistException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status409Conflict, ex.Message);
        }
        catch (KirelAuthenticationException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status401Unauthorized, ex.Message);
        }
        catch (KirelUnauthorizedException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (KirelNotFoundException ex)
        {
            await SetProblemDetails(context, StatusCodes.Status404NotFound, ex.Message);
        }
    }
}