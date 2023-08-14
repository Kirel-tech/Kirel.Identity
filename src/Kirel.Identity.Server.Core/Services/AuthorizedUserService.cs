using AutoMapper;
using Kirel.Identity.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class AuthorizedUserService : KirelAuthorizedUserService<Guid, User, AuthorizedUserDto, AuthorizedUserUpdateDto>
{
    /// <inheritdoc />
    public AuthorizedUserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager,
        IMapper mapper) : base(httpContextAccessor, userManager, mapper)
    {
    }
}