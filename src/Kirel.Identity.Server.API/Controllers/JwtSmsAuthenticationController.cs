using Kirel.Identity.Jwt.Sms.Controllers;
using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Server.API.Controllers;

/// <inheritdoc />
[Route("authentication/jwt")]
[ApiController]
public class JwtSmsAuthenticationController : KirelJwtSmsAuthenticationController<JwtTokenService, SmsAuthenticationService, AuthorizedUserService,
    Guid, User, Role, UserRole, UserClaim, RoleClaim, AuthorizedUserDto, AuthorizedUserUpdateDto>
{
    /// <inheritdoc />
    public JwtSmsAuthenticationController(SmsAuthenticationService authService, JwtTokenService tokenService, AuthorizedUserService authorizedUserProvider) : base(authService, tokenService, authorizedUserProvider)
    {
    }
}