using System.Text;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Http;

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
    /// <param name="next">RequestDelegate</param>
    public KirelErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    /// <summary>
    /// Middleware call point
    /// </summary>
    /// <param name="context">HttpContext</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KirelIdentityStoreException)
        {
            var response = context.Response;
            response.StatusCode = StatusCodes.Status500InternalServerError;
        }
        catch (KirelValidationException)
        {
            var response = context.Response;
            response.StatusCode = StatusCodes.Status400BadRequest;
        }
        catch (KirelAlreadyExistException)
        {
            var response = context.Response;
            response.StatusCode = StatusCodes.Status400BadRequest;
        }
        catch (KirelAuthenticationException)
        {
            var response = context.Response;
            response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        catch (KirelUnauthorizedException)
        {
            var response = context.Response;
            response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        catch (KirelNotFoundException)
        {
            var response = context.Response;
            response.StatusCode = StatusCodes.Status404NotFound;
        }
    }
}