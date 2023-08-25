using AutoMapper;
using Kirel.Identity.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class RegistrationService : KirelRegistrationService<Guid, User, Role, UserRole, UserRegistrationDto>
{
    /// <inheritdoc />
    public RegistrationService(UserManager<User> userManager, IMapper mapper) : base(userManager, mapper)
    {
    }
}