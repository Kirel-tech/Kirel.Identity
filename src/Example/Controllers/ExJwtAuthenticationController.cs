using Example.DTOs;
using Example.Models;
using Example.Services;
using Kirel.Identity.Jwt.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

/// <inheritdoc />
[ApiController]
[Route("authentication/jwt")]
public class ExJwtAuthenticationController : KirelJwtAuthenticationController<ExJwtTokenService,ExAuthenticationService,
    ExAuthorizedUserService,Guid,ExUser,ExRole,ExAuthorizedUserDto, ExAuthorizedUserUpdateDto>
{
    /// <inheritdoc />
    public ExJwtAuthenticationController(ExAuthenticationService authService, ExJwtTokenService tokenService, ExAuthorizedUserService authorizedUserservice) : base(authService, tokenService, authorizedUserservice)
    {
    }
}