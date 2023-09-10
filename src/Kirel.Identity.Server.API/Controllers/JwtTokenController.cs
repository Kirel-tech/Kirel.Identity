using Kirel.Identity.Jwt.Controllers;
using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Server.API.Controllers;

/// <inheritdoc />
[Route("authentication/jwt")]
[ApiController]
public class AuthenticationController : KirelJwtAuthenticationController<JwtTokenService, AuthenticationService,
    AuthorizedUserService, EmailAuthenticationService,Guid, User, Role, UserRole, AuthorizedUserDto, AuthorizedUserUpdateDto>
{
    /// <inheritdoc />
    public AuthenticationController(AuthenticationService authService, JwtTokenService tokenService,
        AuthorizedUserService authorizedUserProvider,EmailAuthenticationService emailAuthenticationService) : base(authService, tokenService, authorizedUserProvider,emailAuthenticationService)
    {
    }
}