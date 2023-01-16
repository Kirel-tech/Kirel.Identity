using AutoMapper;
using Example.DTOs;
using Example.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.Services;

/// <inheritdoc />
public class ExAuthorizedUserService : KirelAuthorizedUserService<Guid, ExUser, ExAuthorizedUserDto, ExAuthorizedUserUpdateDto>
{
    /// <inheritdoc />
    public ExAuthorizedUserService(IHttpContextAccessor httpContextAccessor, UserManager<ExUser> userManager, IMapper mapper) : base(httpContextAccessor, userManager, mapper)
    {
    }
}