using AutoMapper;
using Example.DTOs;
using Example.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.Services;

/// <inheritdoc />
public class ExRegistrationService : KirelRegistrationService<Guid, ExUser, ExUserRegistrationDto>
{
    /// <inheritdoc />
    public ExRegistrationService(UserManager<ExUser> userManager, IMapper mapper) : base(userManager, mapper)
    {
    }
}