using AutoMapper;
using Example.API.Models;
using Example.DTOs;
using Kirel.Identity.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace Example.API.Services;

/// <inheritdoc />
public class ExAuthorizedUserService : KirelAuthorizedUserService<Guid, ExUser, ExAuthorizedUserDto, ExAuthorizedUserUpdateDto>
{
    /// <inheritdoc />
    public ExAuthorizedUserService(IHttpContextAccessor httpContextAccessor, UserManager<ExUser> userManager, IMapper mapper) : base(httpContextAccessor, userManager, mapper)
    {
    }
}