using AutoMapper;
using Example.API.Models;
using Example.DTOs;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.API.Services;

/// <inheritdoc />
public class ExUserService : KirelUserService<Guid, ExUser, ExRole, ExUserDto, ExUserCreateDto, ExUserUpdateDto, KirelClaimDto,
    KirelClaimCreateDto,KirelClaimUpdateDto>
{
    /// <inheritdoc />
    public ExUserService(UserManager<ExUser> userManager, RoleManager<ExRole> roleManager, IMapper mapper) : base(userManager, roleManager, mapper)
    {
    }
}